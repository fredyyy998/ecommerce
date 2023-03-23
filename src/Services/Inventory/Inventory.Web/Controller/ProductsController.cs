using Inventory.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Web;

[ApiController]
[Route("/api/[controller]")]
public class ProductsController : BaseController
{
    public ProductsController(IProductService productService) : base(productService)
    {
    }

    [HttpGet()]
    public IActionResult GetProductsBySearch(string? search)
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
}