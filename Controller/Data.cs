using Microsoft.VisualBasic;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;


namespace Controller
{
    public static class Data
    {
        public static Competition Competition;
        public static Race CurrentRace;

        public static event EventHandler<NextRaceEventArgs> NextRaceEvent;
        public static event EventHandler<NextRaceEventArgs> CompetitionEnded;

        private static bool _lastRace;

        static Data()
        {
            Initialize();
        }

        public static void NextRace()
        {
            CurrentRace?.CleanUp();

            Track track = Competition.NextTrack();
            if(track != null)
            {
                CurrentRace = new Race(track, Competition.Participants);
                CurrentRace.RaceFinished += OnRaceFinished;
                CurrentRace.Start();
                NextRaceEvent?.Invoke(null, new NextRaceEventArgs() { Race = CurrentRace });
            }
            else
            {
                CompetitionEnded?.Invoke(null, new NextRaceEventArgs() { Race = null });
            }
        }

        public static void Initialize()
        {
            Competition = new Competition("Mushroom Cup");
            addParticipants();
            addTracks();
        }

        public static void addParticipants()
        {
            Competition.Participants.Add(new Driver("Yoshi", 0, IParticipant.TeamColors.Green));
            Competition.Participants.Add(new Driver("Mario", 0, IParticipant.TeamColors.Red));
            Competition.Participants.Add(new Driver("Toad", 0, IParticipant.TeamColors.Red));
            Competition.Participants.Add(new Driver("Donkey Kong", 0, IParticipant.TeamColors.Orange));
            Competition.Participants.Add(new Driver("Peach", 0, IParticipant.TeamColors.Yellow));
            Competition.Participants.Add(new Driver("Waluigi", 0, IParticipant.TeamColors.Grey));
        }

        public static void addTracks()
        {
           Competition.Tracks.Enqueue(new Track("Rainbow Road", new Section.SectionTypes[]
            {
                Section.SectionTypes.RightCorner,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.StartGrid,
                Section.SectionTypes.StartGrid,
                Section.SectionTypes.StartGrid,
                Section.SectionTypes.StartGrid,
                Section.SectionTypes.Finish,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.RightCorner,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.RightCorner,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.LeftCorner,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.LeftCorner,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.RightCorner,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.RightCorner,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.RightCorner,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.RightCorner,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.LeftCorner,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.LeftCorner,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.RightCorner,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
            }));
            Competition.Tracks.Enqueue(new Track("Mushroom Village", new Section.SectionTypes[]
            {
                Section.SectionTypes.RightCorner,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.LeftCorner,
                Section.SectionTypes.RightCorner,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.RightCorner,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.RightCorner,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.StartGrid,
                Section.SectionTypes.StartGrid,
                Section.SectionTypes.StartGrid,
                Section.SectionTypes.StartGrid,
                Section.SectionTypes.Finish,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.RightCorner,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
            }));
            Competition.Tracks.Enqueue(new Track("Coconut Mall", new Section.SectionTypes[]
            {
                Section.SectionTypes.RightCorner,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.StartGrid,
                Section.SectionTypes.StartGrid,
                Section.SectionTypes.StartGrid,
                Section.SectionTypes.StartGrid,
                Section.SectionTypes.Finish,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.RightCorner,
                Section.SectionTypes.RightCorner,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.LeftCorner,
                Section.SectionTypes.LeftCorner,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.RightCorner,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.RightCorner,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.RightCorner,
                Section.SectionTypes.Straight,
                Section.SectionTypes.LeftCorner,
                Section.SectionTypes.LeftCorner,
                Section.SectionTypes.RightCorner,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.RightCorner,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
                Section.SectionTypes.Straight,
            }));
        }

        private static void OnRaceFinished(object sender, EventArgs e)
        {
            NextRace();
        }
    }
        
    }
