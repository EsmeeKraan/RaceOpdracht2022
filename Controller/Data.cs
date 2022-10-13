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
            Competition.Participants.Add(new Driver("Yoshi", 0));
            Competition.Participants.Add(new Driver("Mario", 0));
            Competition.Participants.Add(new Driver("Toad", 0));
            Competition.Participants.Add(new Driver("Donkey Kong", 0));
            Competition.Participants.Add(new Driver("Peach", 0));
            Competition.Participants.Add(new Driver("Waluigi", 0));
            Competition.Participants.Add(new Driver("Luigi", 0));
            Competition.Participants.Add(new Driver("Bowser", 0));
        }

        public static void addTracks()
        {
            Competition.Tracks.Enqueue(new Track("Rainbow Road", new Section.SectionTypes[] {
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
                                                                      Section.SectionTypes.RightCorner,
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
                                                                      Section.SectionTypes.Straight,
                                                                    }));
            Competition.Tracks.Enqueue(new Track("Mushroom Village", new Section.SectionTypes[0]));
            Competition.Tracks.Enqueue(new Track("Coconut Mall", new Section.SectionTypes[0]));   
        }
    }
        
    }
