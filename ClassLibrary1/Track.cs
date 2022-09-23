using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Track
    {
        #region Properties
        public string Name { get; set; }
        LinkedList<Section> Sections { get; set; }
        #endregion

        #region Constructors
        public Track(string Name, Section.SectionTypes[] sections)
        {
            this.Name = Name;
            this.Sections = new LinkedList<Section>(); // sections meegeven, conversion doen
        }
        #endregion
    }
}
