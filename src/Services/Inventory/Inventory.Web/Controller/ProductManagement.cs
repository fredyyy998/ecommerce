using Inventory.Application.Dtos;
using Inventory.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Web;

[ApiController]
[Route("/api/[controller]")]
[Authorize(Roles = "Admin")]
public class ProductManagement : BaseController
{
    public ProductManagement(IProductService productService) : base(productService)
    {
    }
    
    [HttpGet("{id}")]
    public IActionResult GetProduct(Guid id)
    {
        try
        {
            var product = _productService.GetAdminProduct(id);
            return Ok(product);
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
    
    [HttpPost("{id}/stock/add")]
    public IActionResult AddStock(Guid id, [FromBody] int quantity)
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
    
    [HttpPost("{id}/stock/remove")]
    public IActionResult RemoveStock(Guid id, [FromBody] int quantity)
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