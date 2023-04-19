using Ecommerce.Common.Core;
using Inventory.Application.Dtos;
using Inventory.Application.Services;
using Inventory.Infrastructure.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;

namespace Inventory.Web;

/// <summary>
/// The product management controller, only accessible by the admin role
/// </summary>
[ApiController]
[Route("/api/[controller]")]
[Authorize(Roles = "Admin")]
[Produces("application/json")]
public class ProductManagement : BaseController
{
    public ProductManagement(IProductService productService) : base(productService)
    {
    }
    
    /// <summary>
    /// Returns a list of products  with pagination in the admin format
    /// </summary>
    /// <param name="productParameters">The pagination parameters</param>
    /// <returns>A paginated list of products</returns>
    /// <response code="200">Returns the paginated list of products</response>
    /// <response code="500">If an internal server error occurs</response>
    [HttpGet()]
    [ProducesResponseType(typeof(PagedList<AdminProductResponseDto>), StatusCodes.Status200OK)]
    [SwaggerResponseHeader(StatusCodes.Status200OK, "X-Pagination", "The pagination information for the response list", "json")]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    
    public async Task<IActionResult> GetProducts([FromQuery] ProductParameters productParameters)
    {
        try
        {
            var products = await _productService.GetAdminProducts(productParameters);
            return Ok(products.Item1);
        }
        catch (Exception e)
        {
            return HandleException(e);
        }
    }
    
    /// <summary>
    /// Returns a product by id
    /// </summary>
    /// <param name="productId">The product object</param>
    /// <returns>The requested product</returns>
    /// <response code="200">Returns the requested product</response>
    /// <response code="401">If the user is not authorized</response>
    /// <response code="404">If the product does not exist</response>
    /// <response code="500">If an internal server error occurs</response>
    [HttpGet("{productId}")]
    [ProducesResponseType(typeof(AdminProductResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProduct(Guid productId)
    {
        try
        {
            var product = await _productService.GetAdminProduct(productId);
            return Ok(product);
        }
        catch (Exception e)
        {
            return HandleException(e);
        }
    }

    /// <summary>
    /// Creates a new product
    /// </summary>
    /// <param name="productRequestDto">The product data</param>
    /// <returns>nothing</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     Post /api/ProductManagement
    ///     {
    ///         "name": "Product 1",
    ///         "description": "Product 1 description",
    ///         "GrossPrice": 10,99,
    ///     }
    ///
    /// </remarks>
    /// <response code="204">If the product was created successfully</response>
    /// <response code="401">If the user is not authorized</response>
    /// <response code="500">If an internal server error occurs</response>
    [HttpPost()]
    public async Task<IActionResult> CreateProduct([FromBody] ProductCreateDto productRequestDto)
    {
        try
        {
            // TODO respond with created product and return CreatedAtAction
            await _productService.CreateProduct(productRequestDto);
            return NoContent();
        }
        catch (Exception e)
        {
            return HandleException(e);
        }
    }
    
    /// <summary>
    /// Updates a product by id
    /// </summary>
    /// <param name="productId">The product id of the to be updated product</param>
    /// <param name="productUpdateDto">The data to be updated</param>
    /// <returns>nothing</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     Patch /api/ProductManagement/{productId}
    ///     {
    ///         "name": "Product 1",
    ///         "description": "Product 1 description",
    ///         "GrossPrice": 10,99,
    ///         "ProductInformation": [
    ///             {   "color": "red"  },
    ///             {   "size": "average"  }
    ///         ]
    ///
    /// </remarks>
    /// <response code="204">If the product was updated successfully</response>
    /// <response code="401">If the user is not authorized</response>
    /// <response code="404">If the product does not exist</response>
    /// <response code="500">If an internal server error occurs</response>
    [HttpPatch("{productId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateProduct(Guid productId, [FromBody] ProductUpdateDto productUpdateDto)
    {
        try
        {
            await _productService.UpdateProduct(productId, productUpdateDto);
            return NoContent();
        }
        catch (Exception e)
        {
            return HandleException(e);
        }
    }
    
    /// <summary>
    /// Deletes a product by id
    /// </summary>
    /// <param name="productId">The product id to be deleted</param>
    /// <returns>nothing</returns>
    /// <response code="204">If the product was deleted successfully</response>
    /// <response code="401">If the user is not authorized</response>
    /// <response code="404">If the product does not exist</response>
    /// <response code="500">If an internal server error occurs</response>
    [HttpDelete("{productId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteProduct(Guid productId)
    {
        try
        {
            await _productService.DeleteProduct(productId);
            return NoContent();
        }
        catch (Exception e)
        {
            return HandleException(e);
        }
    }
    
    /// <summary>
    /// Adds stock to a product
    /// </summary>
    /// <param name="productId">The id of the product</param>
    /// <param name="quantity">The quantity to add to the stock</param>
    /// <returns>nothing</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     Post /api/ProductManagement/{productId}/stock/add
    ///     {
    ///         "quantity": 10
    ///     }
    ///
    /// </remarks>
    /// <response code="204">If the stock was added successfully</response>
    /// <response code="401">If the user is not authorized</response>
    /// <response code="404">If the product does not exist</response>
    /// <response code="500">If an internal server error occurs</response>
    [HttpPost("{productId}/stock/add")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddStock(Guid productId, [FromBody] QuantityDto quantityDto)
    {
        try
        {
            await _productService.AddStock(productId, quantityDto.Quantity);
            return NoContent();
        }
        catch (Exception e)
        {
            return HandleException(e);
        }
    }
    
    /// <summary>
    /// Removes stock from a product
    /// </summary>
    /// <param name="productId">The id of the product to be updated</param>
    /// <param name="quantityDto">The quantity to remove from the products stock</param>
    /// <returns>nothing</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     Post /api/ProductManagement/{productId}/stock/remove
    ///     {
    ///         "quantity": 10
    ///     }
    /// 
    /// </remarks>
    /// <response code="204">If the stock was removed successfully</response>
    /// <response code="401">If the user is not authorized</response>
    /// <response code="404">If the product does not exist</response>
    /// <response code="500">If an internal server error occurs</response>
    [HttpPost("{productId}/stock/remove")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveStock(Guid productId, [FromBody] QuantityDto quantityDto)
    {
        try
        {
            await _productService.RemoveStock(productId, quantityDto.Quantity);
            return NoContent();
        }
        catch (Exception e)
        {
            return HandleException(e);
        }
    }
}