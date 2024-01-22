using System.Transactions;
using bookingservice.dao;
using Dapper;

namespace bookingservice.db;

public class DbManager: DbConnection, IDbManager
{
    /// Constructor
    public DbManager(ILogger logger) : base(logger)
    {
    }
    
    public TransactionScope CreateTransactionScope()
    {
        return new TransactionScope();
    }

    public int GetLastId()
    {
        try
        {
            var query = "SELECT LAST_INSERT_ID()";
            return Conn.ExecuteScalar<int>(query);
        }
        catch (Exception ex)
        {
            Logger.LogError("Error: {Message}", ex.Message);
            throw;
        }
    }
    
    public List<User> GetUsers()
    {
        try
        {
            return Conn.GetList<User>().ToList();
        }
        catch (Exception ex)
        {
            Logger.LogError("Error: {Message}", ex.Message);
            throw;
        }
    }

    public User? GetUserById(int userId)
    {
        try
        {
            return Conn.Get<User>(userId);
        }
        catch (Exception ex)
        {
            Logger.LogError("Error: {Message}", ex.Message);
            throw;
        }    
    }

    public Event? GetEventById(int id)
    {
        try
        {
            return Conn.Get<Event>(id);
        }
        catch (Exception ex)
        {
            Logger.LogError("Error: {Message}", ex.Message);
            throw;
        }  
    }

    public List<Event> GetEvents()
    {
        try
        {
            return Conn.GetList<Event>().ToList();
        }
        catch (Exception ex)
        {
            Logger.LogError("Error: {Message}", ex.Message);
            throw;
        }  
    }

    public int GetOccupiedSeatsForEvent(int eventId)
    {
        try
        {
            string query = $"SELECT SUM(Seats) FROM orderdetail WHERE EventId = {eventId}";
            return Conn.ExecuteScalar<int>(query);
        }
        catch (Exception ex)
        {
            Logger.LogError("Error: {Message}", ex.Message);
            throw;
        }
    }

    public int? InsertOrder(Order order)
    {
        try
        {
            return Conn.Insert(order);
        }
        catch (Exception ex)
        {
            Logger.LogError("Error: {Message}", ex.Message);
            throw;
        }
    }

    public int? InsertOrderDetail(OrderDetail orderDetail)
    {
        try
        {
            return Conn.Insert(orderDetail);
        }
        catch (Exception ex)
        {
            Logger.LogError("Error: {Message}", ex.Message);
            throw;
        }
    }
}