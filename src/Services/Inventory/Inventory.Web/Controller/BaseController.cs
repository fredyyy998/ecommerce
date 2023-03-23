using Inventory.Application.Services;
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
        return StatusCode(StatusCodes.Status500InternalServerError);
    }
}