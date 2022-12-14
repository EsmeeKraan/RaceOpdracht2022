namespace Model
{
    public class Car : IEquipment
    {
        public int Quality { get; set; }
        public int Performance { get; set; }
        public int Speed { get; set; }
        public bool IsBroken { get; set; }

        public Car(int quality, int performance, int speed)
        {
            Quality = quality;
            Performance = performance;
            Speed = speed;
            IsBroken = false;
        }
    }
}
