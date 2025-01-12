using SQLite;

namespace GestionareEven.Models
{
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }  // Unique user ID

        [MaxLength(100)]
        public string Name { get; set; } // User name

        [MaxLength(150)]
        public string Email { get; set; } // User email
    }
}
