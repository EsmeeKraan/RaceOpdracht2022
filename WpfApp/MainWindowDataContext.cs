using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using Controller;
using Model;

namespace WpfApp
{
    using DataContextDispatcher = Action<Action>;
    public class MainWindowDataContext : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public string TrackName { get; set; }
        public string CompetitionName { get; set; }
        protected DataContextDispatcher? _dispatcher;
        public ObservableCollection<string> CompetitionPoints { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> ParticipantLapTime { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> ParticipantSpeed { get; set; } = new ObservableCollection<string>();
        public MainWindowDataContext()
        {
            Data.NextRaceEvent += Data_NextRaceEvent;
        }

        public DataContextDispatcher? Dispatcher
        {
            get => _dispatcher;
            set
            {
                _dispatcher = value;
                if (_dispatcher != null)
                    Data_NextRaceEvent(this, new NextRaceEventArgs());
            }
        }

        private void Data_NextRaceEvent(object? sender, NextRaceEventArgs e)
        {
            if(_dispatcher == null)
            {
                return;
            }

            if(Data.CurrentRace != null)
            {
                Data.CurrentRace.RaceFinished += CurrentRace_RaceFinished;
                Data.CurrentRace.DriversChanged += CurrentRace_Race_DriversChanged;
            }

            _dispatcher((Action)(() =>
            {
                TrackName = Data.CurrentRace?.Track.Name ??"";
                CompetitionName = Data.Competition.Name;
                CompetitionPointsGiving();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
            }));
        }

        private void CurrentRace_RaceFinished(object? sender, EventArgs e)
        {
            if (_dispatcher == null)
            {
                return;
            }

            _dispatcher((Action)(() =>
            {
                CompetitionPointsGiving();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
            }));
        }

        private void CurrentRace_Race_DriversChanged(object? sender, EventArgs e)
        {
            if (_dispatcher == null)
            {
                return;
            }

            _dispatcher((Action)(() =>
            {
                LapsChanged();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
            }));
        }

        private void LapsChanged()
        {
            ParticipantLapTime.Clear();
            ParticipantLapTime.Add("Participant laptime");
            foreach (IParticipant participant in Data.Competition.Participants)
            {
                string lapTime = "";
                if (Data.CurrentRace.ParticipantLapTime.ContainsKey(participant))
                {
                    lapTime = (Data.CurrentRace.ParticipantLapTime[participant] - Data.CurrentRace.StartTime).ToString();
                }
                ParticipantLapTime.Add(participant.Name + " " + lapTime);
            }
        }

        private void CompetitionPointsGiving()
        {
            CompetitionPoints.Clear();
            ParticipantSpeed.Clear();
            CompetitionPoints.Add("Participant Points");
            ParticipantSpeed.Add("Participant Speed");
            foreach (IParticipant participant in Data.Competition.Participants)
            {
                ParticipantSpeed.Add(participant.Name + " " + participant.Equipment.Speed);
                CompetitionPoints.Add(participant.Name + " " + participant.Points);
            }
        }
    }
}
