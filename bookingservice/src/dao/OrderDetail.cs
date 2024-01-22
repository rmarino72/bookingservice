using Dapper;

namespace bookingservice.dao;

[Table("OrderDetail")]
public class OrderDetail
{
    [Column("Id")]
    [Key]
    public required int Id { set; get; }
    [Column("OrderId")]
    public required int OrderId { set; get; }
    [Column("EventId")]
    public required int EventId { set; get; }
    [Column("Seats")]
    public required int Seats { set; get; }
}