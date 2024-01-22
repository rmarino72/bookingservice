using Dapper;

namespace bookingservice.dao;

[Table("event")]
public class Event
{
    [Key]
    [Column("Id")]
    public required int Id { set; get; }
    [Column("Name")]
    public required string Name { set; get; }
    [Column("Description")]
    public required string Description { set; get; }
    [Column("EventDateTime")]
    public required DateTime EventDateTime { set; get; }
    [Column("TotalSeats")]
    public required int TotalSeats { set; get; }
    [Column("Price")]
    public required float Price { set; get; }
}