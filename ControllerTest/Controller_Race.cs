using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Controller;
using Model;
using static Model.IParticipant;
using static Model.Section;

namespace ControllerTest
{
    [TestFixture]
    public class Controller_Race
    {
        private Race _testRace { get; set; }
        private Track _track { get; set; }
        private List<IParticipant> _participants { get; set; }

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
        public void CheckIfCompetitionExists()
        {
            Data.Initialize();
            var result = Data.Competition;
            Assert.IsNotNull(result);
        }

        [Test]
        public void CheckIfParticipantsAdded()
        {
            Data.Initialize();
            var result = Data.CurrentRace.Participants;
            Assert.IsNotNull(result);
        }

        public void CheckIfTracksAdded()
        {
            Data.Initialize();
            var result = Data.CurrentRace.Track;
            Assert.IsNotNull(result);
        }

        [Test]
        public void CheckForAtleast6Participants()
        {
            _track = new Track("TestTrack", new Section.SectionTypes[] { Section.SectionTypes.Straight });
            _participants = new List<IParticipant>();

            _participants.Add(new Driver("Fake Mario", 0, TeamColors.Blue));
            _participants.Add(new Driver("Fake Bowser", 0, TeamColors.Orange));
            _participants.Add(new Driver("Fake Peach", 0, TeamColors.Pink));
            _participants.Add(new Driver("Fake luigi", 0, TeamColors.Green));
            _participants.Add(new Driver("Fake Wario", 0, TeamColors.Red));
            _participants.Add(new Driver("Fake Yoshi", 0, TeamColors.DarkGreen));
            _participants.Add(new Driver("Fake Toad", 0, TeamColors.White));

            _testRace = new Race(_track, _participants);
            _testRace.SetDriverStartPosition(_track, _participants);


            Assert.That(_participants.Count, Is.GreaterThanOrEqualTo(6));
        }

        [Test]
        public void CheckIfLessThan6Participants()
        {

            _track = new Track("TestTrack", new Section.SectionTypes[] { Section.SectionTypes.Straight });
            _participants = new List<IParticipant>();

            _participants.Add(new Driver("Fake Mario", 0, TeamColors.Red));
            _participants.Add(new Driver("Fake Bowser", 0, TeamColors.Orange));
            _participants.Add(new Driver("Fake Peach", 0, TeamColors.Pink));
            _participants.Add(new Driver("Fake luigi", 0, TeamColors.Green));
            _participants.Add(new Driver("Fake Dry Bones", 0, TeamColors.Blue));

            _testRace = new Race(_track, _participants);
            _testRace.SetDriverStartPosition(_track, _participants);

            Assert.That(_participants.Count, Is.LessThanOrEqualTo(6));
        }

        [Test]
        public void TestParticipantFinished()
        {
            bool Finished = false;

            Data.CurrentRace.RaceFinished += (_, _) =>
            {
                Finished = true;
                foreach (IParticipant participant in Data.CurrentRace.Participants)
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

        [Test]
        public void GetSectionData_ReturnsData()
        {
            _track = new Track("test", new SectionTypes[] { SectionTypes.StartGrid, SectionTypes.Straight, SectionTypes.LeftCorner, SectionTypes.RightCorner, SectionTypes.Finish });
            _participants = new List<IParticipant>();
            _participants.Add(new Driver("Fake Yoshi", 0,TeamColors.Blue));
            _testRace = new Race(_track, _participants);

            foreach (var section in Data.CurrentRace.Track.Sections)
            {
                var SectionData = Data.CurrentRace.GetSectionData(section);
                Assert.NotNull(SectionData);
            }
        }
    }
}
