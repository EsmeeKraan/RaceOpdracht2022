using Controller;
using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

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
        public ObservableCollection<string> CompetitionCompetitors { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> CompetitionTrackList { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> ParticipantLapTime { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> ParticipantTeamColor { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> ParticipantSpeed { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> ParticipantPerformance { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> CompetitionWinner { get; set; } = new ObservableCollection<string>();
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
            if (_dispatcher == null)
            {
                return;
            }

            if (Data.CurrentRace != null)
            {
                Data.CurrentRace.RaceFinished += CurrentRace_RaceFinished;
                Data.CurrentRace.DriversChanged += CurrentRace_Race_DriversChanged;
            }

            _dispatcher((Action)(() =>
            {
                TrackName = Data.CurrentRace?.Track.Name ?? "";
                CompetitionName = Data.Competition.Name;
                AddInfoToScreens();
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
                AddInfoToScreens();
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
            ParticipantLapTime.Add("Participant Total Laptime");
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

        private void AddInfoToScreens()
        {
            CompetitionCompetitors.Clear();
            CompetitionTrackList.Clear();
            ParticipantPerformance.Clear();
            CompetitionPoints.Clear();
            ParticipantSpeed.Clear();
            ParticipantTeamColor.Clear();
            CompetitionTrackList.Add("Upcoming Tracks!");
            ParticipantTeamColor.Add("Participant Color");
            CompetitionPoints.Add("Participant Points");
            ParticipantSpeed.Add("Participant Speed");
            ParticipantPerformance.Add("Participant Car Performance");
            CompetitionCompetitors.Add("Competitors Competing in Competition");
            foreach (IParticipant participant in Data.Competition.Participants)
            {
                ParticipantSpeed.Add(participant.Name + " " + participant.Equipment.Speed);
                ParticipantPerformance.Add($"{participant.Name} {participant.Equipment.Performance}");
;               CompetitionPoints.Add(participant.Name + " " + participant.Points);
                ParticipantTeamColor.Add($"{participant.Name}: {participant.TeamColor}");
                CompetitionCompetitors.Add($"{participant.Name}");
            }


            foreach(Track track in Data.Competition.Tracks)
            {
                CompetitionTrackList.Add(track.Name);
            }
            
        }

        
    }
}
