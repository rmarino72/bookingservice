
namespace bookingservice.exception;

public sealed class BookingServiceException: Exception
{
    public BookingServiceException(string message) : base(message)
    {
        Data.Add("BookingService", "true");
    }
}