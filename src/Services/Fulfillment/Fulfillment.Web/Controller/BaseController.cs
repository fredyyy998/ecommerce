using Fulfillment.Application.Exceptions;
using Fulfillment.Core.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Fulfillment.Web;

public abstract class BaseController : Controller
{
    protected IActionResult HandleException(Exception exception)
    {
        switch (exception)
        {
            case EntityNotFoundException _:
                return NotFound(exception.Message);
            case OrderDomainException _:
                return Forbid(exception.Message);
            default:
                return BadRequest(exception.Message);
        }
    }
}