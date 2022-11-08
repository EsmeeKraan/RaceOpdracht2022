namespace Controller
{
    /// <summary>
    /// Class for the NextRaceEvent, Contains an object from the Class Race
    /// </summary>
    public class NextRaceEventArgs : EventArgs
    {
        public Race Race { get; set; }
    }
}