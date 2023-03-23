using Inventory.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Web;

[ApiController]
[Route("/api/[controller]")]
public class ProductsController : Controller
{
    private readonly IProductService _productService;
    
    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }
    
    [HttpGet()]
    public IActionResult GetProductsBySearch(string search)
    {
        try
        {
            var products = _productService.SearchProduct(search);
            return Ok(products);
        }
        catch (Exception e)
        {
            return HandleException(e);
        }
    }
    
    private IActionResult HandleException(Exception e)
    {
        return StatusCode(StatusCodes.Status500InternalServerError);
    }
}