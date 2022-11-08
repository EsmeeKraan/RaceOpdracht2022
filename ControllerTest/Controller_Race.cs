using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Controller;
using Model;

namespace ControllerTest
{
    [TestFixture]
    public class Controller_Race
    {
        [SetUp]
        public void SetUp()
        {
            Data.Initialize();
            Data.NextRace();
        }

        [TearDown]
        public void TearDown()
        {
            Data.CurrentRace.CleanUp();
            Data.CurrentRace = null;
        }

        [Test]
        public void TestParticipantFinished()
        {
            bool Finished = false;

            Data.CurrentRace.RaceFinished += (_,_) =>
            {
                Finished = true;
                foreach(IParticipant participant in Data.CurrentRace.Participants)
                {
                    Assert.That(participant.Points, Is.Not.Zero);
                }
            };

            Thread.Sleep(60000);
            Assert.That(Finished, Is.True);
        }

        [Test]
        public void CheckParticipantsCrossedFinish()
        {
            Dictionary<IParticipant, bool> Participant = new Dictionary<IParticipant, bool>();
            Dictionary<IParticipant, Section> Positions = new Dictionary<IParticipant, Section>();

            foreach (IParticipant participant in Data.CurrentRace.Participants)
            {
                Participant[participant] = false;
                Positions[participant] = new Section(Section.SectionTypes.Straight);
            }

            Data.CurrentRace.DriversChanged += (_, _) =>
            {
                foreach (var section in Data.CurrentRace.Track.Sections)
                {
                    var SectionData = Data.CurrentRace.GetSectionData(section);
                    if (SectionData.Left != null)
                    {
                        if (Positions[SectionData.Left] != section && Positions[SectionData.Left].SectionType != section.SectionType)
                        {
                            if (Positions[SectionData.Left].SectionType == Section.SectionTypes.Finish)
                            {
                                Participant[SectionData.Left] = true;
                            }
                            Positions[SectionData.Left] = section;
                        }
                    }

                    if (SectionData.Right != null)
                    {
                        if (Positions[SectionData.Right] != section && Positions[SectionData.Right].SectionType != section.SectionType)
                        {
                            if (Positions[SectionData.Right].SectionType == Section.SectionTypes.Finish)
                            {
                                Participant[SectionData.Right] = true;
                            }
                            Positions[SectionData.Right] = section;
                        }
                    }
                }
            };

            Data.CurrentRace.RaceFinished += (_, _) =>
            {
                foreach (IParticipant participant in Data.CurrentRace.Participants)
                {
                    Assert.That(participant.Points, Is.Not.Zero);
                }
            };

            Thread.Sleep(60000);

            foreach (IParticipant participant in Data.CurrentRace.Participants)
            {
                Assert.That(Participant[participant], Is.True);
            }
        }

    }
}
