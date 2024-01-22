using System.Transactions;
using bookingservice.dao;

namespace bookingservice.db;

public interface IDbManager
{
    public TransactionScope CreateTransactionScope();
    public int GetLastId();
    public List<User> GetUsers();
    public User? GetUserById(int userId);
    public List<Event> GetEvents();
    public Event? GetEventById(int id);
    public int GetOccupiedSeatsForEvent(int eventId);
    public int? InsertOrder(Order order);
    public int? InsertOrderDetail(OrderDetail orderDetail);
}