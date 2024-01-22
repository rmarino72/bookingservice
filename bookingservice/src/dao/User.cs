using Dapper;

namespace bookingservice.dao;

[Table("user")]
public class User
{
    [Key]
    [Column("Id")]
    public required int Id { set; get; }
    [Column("FirstName")]
    public required string FirstName { set; get; }
    [Column("LastName")]
    public required string LastName { set; get; }
    [Column("Email")]
    public required string Email { set; get; }
}