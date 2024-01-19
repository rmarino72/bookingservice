using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bookingservice.Controllers;

[Authorize]
public class BookingController : Controller
{
     [HttpGet]
     [Route("/")]
     public string Root()
     {
          return "OK";
     }
}