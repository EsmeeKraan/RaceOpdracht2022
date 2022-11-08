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
