using SQLite;

namespace GestionareEven.Models
{
    public class Event
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }  // Unique event ID

        [MaxLength(200)]
        public string Title { get; set; }  // Event title

        public string Description { get; set; }  // Event description

        public DateTime EventDate { get; set; }  // Event date

        public int MaxParticipants { get; set; }  // Maximum participants allowed
    }
}
