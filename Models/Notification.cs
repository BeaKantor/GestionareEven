using SQLite;

namespace GestionareEven.Models
{
    public class Notification
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }  // Unique notification ID

        public string Message { get; set; }  // Notification message

        public DateTime NotifyAt { get; set; }  // Date and time of the notification

        [Indexed]
        public int EventID { get; set; }  // Foreign key referencing Event
    }
}
