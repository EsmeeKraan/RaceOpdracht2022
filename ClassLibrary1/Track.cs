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
        public LinkedList<Section> Sections { get; set; }

        #endregion

        #region Constructors
        public Track(string Name, Section.SectionTypes[] sections)
        {
            this.Name = Name;
            this.Sections = AddSection(sections); // sections meegeven, conversion doen

        }
        #endregion

        public static LinkedList<Section> AddSection(Section.SectionTypes[] sections)
        {
            LinkedList<Section> sectionResultaat = new LinkedList<Section>();
            for (int i = 0; i < sections.Length; i++)
            {
                sectionResultaat.AddLast(new Section(sections[i]));
            }
            return sectionResultaat;
        }
    }
}
