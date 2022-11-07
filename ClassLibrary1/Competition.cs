using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Competition
    {
        #region Properties
        public List<IParticipant> Participants = new();
        public Queue<Track> Tracks = new();
        public string Name;
        #endregion

        #region Methodes
        public Track NextTrack() {
            if(Tracks.Count == 0)
            {
                return null;
            }
            return Tracks.Dequeue();
        }

        public Competition(string name)
        {
            Name = name;
        }
        #endregion
    }
}
