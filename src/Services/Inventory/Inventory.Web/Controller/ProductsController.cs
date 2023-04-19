using System.Text.Json;
using Ecommerce.Common.Core;
using Inventory.Application.Dtos;
using Inventory.Application.Services;
using Inventory.Infrastructure.Repository;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;

namespace Inventory.Web;

[ApiController]
[Route("/api/[controller]")]
[Produces("application/json")]
public class ProductsController : BaseController
{
    public ProductsController(IProductService productService) : base(productService)
    {
    }

    /// <summary>
    /// Gets a paginated list of products by search criteria, when there is no search criteria, it returns all products
    /// </summary>
    /// <param name="productParameters">The pagination parameters</param>
    /// <returns>A paginated list of products</returns>
    /// <response code="200">Returns the paginated list of products</response>
    /// <response code="500">If an internal server error occurs</response>
    [HttpGet()]
    [ProducesResponseType(typeof(PagedList<ProductResponseDto>), StatusCodes.Status200OK)]
    [SwaggerResponseHeader(StatusCodes.Status200OK, "X-Pagination", "The pagination information for the response list", "json")]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetProductsBySearch([FromQuery] ProductParameters productParameters)
    {
        try
        {
            var products = await _productService.GetProducts(productParameters);
            
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(products.Item2));
            return Ok(products.Item1);
        }
        catch (Exception e)
        {
            return HandleException(e);
        }
    }
}