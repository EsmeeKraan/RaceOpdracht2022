namespace Model
{
    public class Section
    {
        #region Properties
        public SectionTypes SectionType { get; set; }
        #endregion

        public Section(SectionTypes sectiontype)
        {
            SectionType = sectiontype;
        }

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
