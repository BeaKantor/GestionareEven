using SQLite;

namespace GestionareEven.Models
{
    public class Event
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime EventDate { get; set; } // Date only
        public TimeSpan EventTime { get; set; } // Add this for the time of the event
        public string Location { get; set; }
        public int MaxParticipants { get; set; }
        public int ReservedSpots { get; set; }
        public string Category { get; set; }
    }



}
