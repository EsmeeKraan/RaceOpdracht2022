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
        static Competition competition;

        static Data()
        {
            Initialize();
        }

        public static void Initialize()
        {
            competition = new Competition();
            addParticipants();
        }

        public static void addParticipants()
        {
            Driver driver = new Driver(Name, Points);
            competition.Participants.Add((IParticipant)driver);
        }

        public static void addTracks(Name, Section)
        {
            Track track = new Track(Name, Section);
            competition.Sections.Add(Track);
        }
    }
        
    }
