using Inventory.Application.Dtos;
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
    
    [HttpPost()]
    public IActionResult CreateProduct([FromBody] ProductCreateDto request)
    {
        try
        {
            _productService.CreateProduct(request);
            return Ok();
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
    
    [HttpPatch("{id}")]
    public IActionResult UpdateProduct(Guid id, [FromBody] ProductUpdateDto request)
    {
        try
        {
            _productService.UpdateProduct(id, request);
            return Ok();
        }
        catch (Exception e)
        {
            return HandleException(e);
        }
    }
    
    [HttpDelete("{id}")]
    public IActionResult DeleteProduct(Guid id)
    {
        try
        {
            _productService.DeleteProduct(id);
            return Ok();
        }
        catch (Exception e)
        {
            return HandleException(e);
        }
    }
    
    [HttpPut("{id}/stock/add")]
    public IActionResult AddStock(Guid id, int quantity)
    {
        try
        {
            _productService.AddStock(id, quantity);
            return Ok();
        }
        catch (Exception e)
        {
            return HandleException(e);
        }
    }
    
    [HttpPut("{id}/stock/remove")]
    public IActionResult RemoveStock(Guid id, int quantity)
    {
        try
        {
            _productService.RemoveStock(id, quantity);
            return Ok();
        }
        catch (Exception e)
        {
            return HandleException(e);
        }
    }
}