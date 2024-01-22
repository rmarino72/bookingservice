using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace bookingservice.Controllers;

public class ExceptionController : Controller
{
    // GET
    [ApiExplorerSettings(IgnoreApi = true)] // Attribute to ignore this method in Swagger
    [Route("error")]
    public IActionResult HandleError()
    {
        var exceptionHandlerFeature =
            HttpContext.Features.Get<IExceptionHandlerPathFeature>()!;
        var httpStatus = HttpStatusCode.InternalServerError;
        if (exceptionHandlerFeature.Error.Data.Contains("BookingService"))
        {
            httpStatus = HttpStatusCode.BadRequest;
        }
        // TODO: Manage http status here
        
        Response.StatusCode = (int)httpStatus;
        return Problem(detail: exceptionHandlerFeature.Error.StackTrace,
            title: exceptionHandlerFeature.Error.Message, statusCode: (int)httpStatus);
    }
}