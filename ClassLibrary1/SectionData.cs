using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class SectionData
    {
        #region Properties
        public IParticipant? Left { get; set; }
        public int DistanceLeft { get; set; }
        public IParticipant? Right { get; set; }
        public int DistanceRight { get; set; }
        #endregion

        public IParticipant? GetParticipant(Side side)
        {
            switch (side)
            {
                case Side.Left:
                    return Left;
                case Side.Right:
                    return Right;
            }
            return null;
        }
    }
}
