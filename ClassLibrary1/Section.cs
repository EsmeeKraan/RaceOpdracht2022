using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Section
    {
        #region Properties
        public SectionTypes SectionType { get; set; }
        #endregion

        #region Enum
        public enum SectionTypes
        {
            Straight,
            LeftCorner,
            RightCorner,
            StartGrid,
            Finish
        }
        #endregion


    }
}
