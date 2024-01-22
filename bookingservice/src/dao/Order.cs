using Dapper;

namespace bookingservice.dao;

[Table("order")]
public class Order
{
    [Column("Id")]
    [Key]
    public required int Id { set; get; }
    [Column("OrderDateTime")]
    public required DateTime OrderDateTime { set; get; }
    [Column("UserId")]
    public required int UserId { set; get; }
    [Column("Total")]
    public required float Total { set; get; }
}