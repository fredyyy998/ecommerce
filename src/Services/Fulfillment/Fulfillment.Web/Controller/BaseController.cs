using Fulfillment.Application.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Fulfillment.Web;

public abstract class BaseController : Controller
{
    protected IActionResult HandleException(Exception exception)
    {
        if (exception is EntityNotFoundException)
        {
            return NotFound(exception.Message);
        }
        return BadRequest(exception.Message);
    }
}