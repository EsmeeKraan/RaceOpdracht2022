using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ControllerTest
{
    [TestFixture]

    public class Model_Competition_NextTrackShould
    {
        Competition _competition;

        [SetUp]
        public void SetUp()
        {
            _competition = new Competition();
        }

        [Test]
        public void NextTrack_EmptyQueue_ReturnNull()
        {
            var result = _competition.NextTrack();
            Assert.IsNull(result);
        }
        [Test]

        public void NextTrack_OneInQueue_ReturnTrack()
        {
            Track track = new Track("Mushroom City", new Section.SectionTypes[0]);
            _competition.Tracks.Enqueue(track);
            var result = _competition.NextTrack();
            Assert.AreEqual(track, result);
        }
        [Test]

        public void NextTrack_OneInQueue_RemoveTrackFromQueue()
        {
            Track track = new Track("Shertbert Land", new Section.SectionTypes[0]);
            _competition.Tracks.Enqueue(track);
            var result = _competition.NextTrack();
            result = _competition.NextTrack();
            Assert.AreEqual(track, result);

        }
        [Test]

        public void NextTrack_TwoInQueue_ReturnNextTrack()
        {
            Track track = new Track("Yoshi Falls", new Section.SectionTypes[0]);
            Track track2 = new Track("Shy Guy Beach", new Section.SectionTypes[0]);
            _competition.Tracks.Enqueue(track);

            Track result = _competition.NextTrack();
            _competition.NextTrack();

            Assert.AreEqual(track, result);

        }
    }
}
