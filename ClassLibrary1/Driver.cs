namespace Model
{
    public class Driver : IParticipant
    {
        #region Properties
        public string Name { get; set; }

        public int Points { get; set; }

        public IEquipment Equipment { get; set; }

        public IParticipant.TeamColors TeamColor { get; set; }
        #endregion

        #region Constructors
        public Driver(string name, int points, IParticipant.TeamColors teamcolor)
        {
            Name = name;
            Points = points;
            TeamColor = teamcolor;
        }
        #endregion
    }
}
