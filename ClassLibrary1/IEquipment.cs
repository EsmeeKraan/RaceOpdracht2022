namespace Model
{
    public interface IEquipment
    {
        #region Properties
        int Quality { get; set; }
        int Performance { get; set; }
        int Speed { get; set; }
        bool IsBroken { get; set; }
        #endregion
    }
}
