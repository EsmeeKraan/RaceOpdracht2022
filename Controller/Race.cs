using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controller
{
    public class Race
    {
        public Track Track;
        public List<IParticipant> Participants;
        public DateTime StartTime;

        private Random _random;
        private Dictionary<Section, SectionData> _positions;

        public Race(Track track, List<IParticipant> participants)
        {
            Track = track;
            Participants = participants;
            StartTime = DateTime.Now;
            _random = new(DateTime.Now.Millisecond);
            _positions = new();
        }

        public SectionData getSectionData(Section section)
        {
            if(_positions.TryGetValue(section, out SectionData? data))
            {
                return data;
            } else
            {
                SectionData nieuweSD = new SectionData();
                _positions.Add(section, nieuweSD);
                return nieuweSD;
            }
        }

        public void RandomizeEquipment()
        {
            foreach (var participant in Participants)
            {
                participant.Equipment.Quality = _random.Next();
                participant.Equipment.Performance = _random.Next();
            }
        }
    }
}
