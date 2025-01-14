using SQLite;

public class Event
{
    [PrimaryKey, AutoIncrement]
    public int ID { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime EventDate { get; set; } = DateTime.Now;
    public TimeSpan EventTime { get; set; } = TimeSpan.Zero;
    public string Location { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public int MaxParticipants { get; set; }
    public int ReservedSpots { get; set; }

    // New Property
    public int CreatorID { get; set; } // References the User.ID of the creator
}
