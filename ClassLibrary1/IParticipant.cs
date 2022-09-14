using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public interface IParticipant
    {
        string Name { get; }
        int Points { get; }
        IEquipment Equipment { get; }
        TeamColors TeamColor { get; }

        public enum TeamColors
        {
            Red,
            Green,  
            Blue,  
            Yellow,
            Grey
        }

    }
}
