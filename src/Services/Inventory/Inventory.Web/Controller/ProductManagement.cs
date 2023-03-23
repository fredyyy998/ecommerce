using Inventory.Application.Dtos;
using Inventory.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Web;

[ApiController]
[Route("/api/[controller]")]
public class ProductManagement : BaseController
{
    public ProductManagement(IProductService productService) : base(productService)
    {
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