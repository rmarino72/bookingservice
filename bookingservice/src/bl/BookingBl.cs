using bookingservice.bo;
using bookingservice.dao;
using bookingservice.db;
using bookingservice.exception;
using bookingservice.service;

namespace bookingservice.bl;

public class BookingBl
{
    private readonly ILogger _logger;
    private readonly IDbManager _db;
    
    public BookingBl(ILogger logger, IDbManager db)
    {
        _logger = logger;
        _db = db;
    }

    public List<User> GetUsers()
    {
        try
        {
            return _db.GetUsers();
        }
        catch (Exception ex)
        {
            _logger.LogError("Error: {Message}", ex.Message);
            throw;
        } 
    }

    public List<Event> GetEvents()
    {
        try
        {
            return _db.GetEvents().Where(x => x.EventDateTime >= DateTime.Now).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError("Error: {Message}", ex.Message);
            throw;
        } 
    }

    public OrderResult RequestOrder(OrderRequest request)
    {
        try
        {
            
            var user = _db.GetUserById(request.UserId);
            float total = (float) 0.0;
            
            if (user is null)
                throw new BookingServiceException($"user with Id {request.UserId} not found");
            
            foreach (var detail in request.Details)
            {
                var ev = _db.GetEventById(detail.EventId);
                
                // check if The event does exists
                if (ev is null)
                    throw new BookingServiceException($"Event with Id {detail.EventId} not found");
                
                // check if the event is not expired
                if(ev.EventDateTime <= DateTime.Now)
                    throw new BookingServiceException($"Event  {ev.Name} is expired");
                
                // check if no more than 3 seats are going to be booked
                if (detail.Seats > 3)
                    return new OrderResult{Message = "It is not possible to order more than 3 seats per event", Outcome = false};
                
                // check if there are enough available seats for the event
                if (_db.GetOccupiedSeatsForEvent(ev.Id) + detail.Seats > ev.TotalSeats)
                    return new OrderResult{Message = $"There are not enugh available seats for the event {ev.Name}", Outcome = false};

                total += ev.Price;
            }

            int? orderId;
            using (var scope = _db.CreateTransactionScope())
            {
                _db.InsertOrder(new Order
                {
                    Id = -1,
                    Total = total,
                    UserId = user.Id,
                    OrderDateTime = DateTime.Now
                });

                orderId = _db.GetLastId();

                if (orderId is null) throw new Exception("Error in database");
                
                foreach (var detail in request.Details)
                {
                    _db.InsertOrderDetail(new OrderDetail
                    {
                       Id = -1,
                       EventId = detail.EventId,
                       OrderId = (int)orderId,
                       Seats = detail.Seats
                    });
                }
                SendConfirmationMail(user.Email, (int)orderId);
                // Commit only if every data is stored and the notification is correctly sent
                scope.Complete();
            }

            return new OrderResult{Message = $"Order processed with id {orderId}", Outcome = true};
        }
        catch (Exception ex)
        {
            _logger.LogError("Error: {Message}", ex.Message);
            throw;
        } 
    }

    private void SendConfirmationMail(string address, int orderId)
    {
        try
        {
            var client = new EmailManager(_logger);
            var text =
                $"Dear customer,\n we are happy to confirm that your order #{orderId} has been processed.\n Best regards.\n The booking service Staff.";
            client.SendEmail(address, "Order confirmation", text);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error: {Message}", ex.Message);
            throw;
        } 
    }
}