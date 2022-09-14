using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Competition
    {
        #region Variabelen
        public List<IParticipant> Participants;
        public Queue<Track> Tracks;
        #endregion

        #region Methodes
        public Track NextTrack() {
            return Tracks.Dequeue();
        }
        #endregion
    }
}
