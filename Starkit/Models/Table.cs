namespace Starkit.Models
{
    public enum State
    {
        Available,
        Booked
    }

    public enum Location
    {
        Window,
        Middle,
        Outdoor,
        Regular
    }
    
    public class Table
    {
        public int Id { get; set; }
        public int Capacity { get; set; }
        public string IconUrl { get; set; }
        public State State { get; set; } = State.Available;
        public string Desc { get; set; }
        public Location Location { get; set; } = Location.Regular;
        public bool IsSmoking { get; set; } = false;
        public bool IsQuiet { get; set; } = true;
        public int Floor { get; set; } = 1;
        public int RestaurantId { get; set; }
        public Restaurant Restaurant { get; set; }
    }
}