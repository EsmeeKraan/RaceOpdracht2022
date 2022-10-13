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

        static Data()
        {
            Initialize();
        }

        public static void NextRace()
        {
            Track track = Competition.NextTrack();
            if(track != null)
            {
                CurrentRace = new Race(track, Competition.Participants);
            }
        }

        public static void Initialize()
        {
            Competition = new Competition();
            addParticipants();
            addTracks();
        }

        public static void addParticipants()
        {
            Competition.Participants.Add(new Driver("Stinksok", 15));
        }

        public static void addTracks()
        {
            Competition.Tracks.Enqueue(new Track("Rainbow Road", new Section.SectionTypes[0]));
            Competition.Tracks.Enqueue(new Track("Mushroom Village", new Section.SectionTypes[0]));
            Competition.Tracks.Enqueue(new Track("Coconut Mall", new Section.SectionTypes[0]));   
            

        }
    }
        
    }
