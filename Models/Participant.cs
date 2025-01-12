using SQLite;

namespace GestionareEven.Models
{
    public class Participant
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }  // Unique participant ID

        [Indexed]
        public int EventID { get; set; }  // Foreign key referencing Event

        [Indexed]
        public int UserID { get; set; }  // Foreign key referencing User
    }
}
