using bookingservice.bl;
using bookingservice.bo;
using bookingservice.dao;
using bookingservice.db;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace bookingservice.Controllers;

[Authorize]
public class BookingController : Controller
{

     private readonly ILogger _logger;
     private readonly IDbManager _db;

     private BookingBl Bl => new BookingBl(_logger, _db);
     public BookingController(ILogger logger, IDbManager db)
     {
          _logger = logger;
          _db = db;
     }

     [HttpGet]
     [Route("/booking/user")]
     [SwaggerOperation(Summary = "returns the list of subscribed users")]
     public List<User> GetUsers()
     { 
          return Bl.GetUsers();
     }
     
     [HttpGet]
     [Route("/booking/event")]
     [SwaggerOperation(Summary = "returns the list of next available events")]
     public List<Event> GetEvents()
     { 
          return Bl.GetEvents();
     }

     [HttpPost]
     [Route("/booking/request-order/{userId}")]
     [SwaggerOperation(Summary = "Places a new order")]
     public OrderResult RequestOrder(int userId, [FromBody] List<OrderRequestDetail> details)
     {
          return Bl.RequestOrder(new OrderRequest { UserId = userId, Details = details });
     }
}