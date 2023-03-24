using Inventory.Application.Exceptions;
using Inventory.Application.Services;
using Inventory.Core;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Web;

public abstract class BaseController : Controller
{
    protected readonly IProductService _productService;
    
    protected BaseController(IProductService productService)
    {
        _productService = productService;
    }
    
    protected IActionResult HandleException(Exception e)
    {
        if (e is ProductDomainException)
        {
            return BadRequest(e.Message);
        }
        else if (e is EntityNotFoundException)
        {
            return NotFound(e.Message);
        }
        
        return StatusCode(StatusCodes.Status500InternalServerError);
    }
}