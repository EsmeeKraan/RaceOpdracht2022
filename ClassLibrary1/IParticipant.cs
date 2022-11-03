using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public interface IParticipant
    {
        #region Properties
        string Name { get; }
        int Points { get; }
        IEquipment Equipment { get; set;  }
        TeamColors TeamColor { get; }
        #endregion

        #region Enums

        public enum TeamColors
        {
            Red,
            Green,  
            Blue,  
            Yellow,
            Grey,
            Orange
        }
        #endregion

    }
}
