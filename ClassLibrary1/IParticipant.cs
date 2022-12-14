namespace Model
{
    public interface IParticipant
    {
        #region Properties
        string Name { get; }
        int Points { get; set; }
        IEquipment Equipment { get; set; }
        TeamColors TeamColor { get; }

        #endregion

        #region Enums

        public enum TeamColors
        {
            Red,
            White,
            Green,
            Blue,
            Pink,
            Brown,
            DarkGreen,
            Orange,
        }
        #endregion

    }
}
