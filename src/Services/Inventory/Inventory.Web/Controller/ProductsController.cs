using System.Text.Json;
using Inventory.Application.Dtos;
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
    public IActionResult GetProductsBySearch([FromQuery] ProductParameters productParameters)
    {
        try
        {
            var products = _productService.GetProducts(productParameters, out var metadata);
            
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(metadata));
            return Ok(products);
        }
        catch (Exception e)
        {
            return HandleException(e);
        }
    }
}