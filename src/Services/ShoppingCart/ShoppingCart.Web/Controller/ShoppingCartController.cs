using System.Security.Claims;
using Account.Application.Exceptions;
using Inventory.Application.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Application.Dtos;
using ShoppingCart.Application.Services;
using ShoppingCart.Core.Exceptions;

namespace ShoppingCart.Web;

/// <summary>
/// The shopping cart controller is responsible for handling all requests related to the shopping cart.
/// </summary>
[ApiController]
[Authorize]
[Route("/api/[controller]")]
[Produces("application/json")]
public class ShoppingCartController : Controller
{
    private readonly IShoppingCartService _shoppingCartService;
    
    public ShoppingCartController(IShoppingCartService shoppingCartService)
    {
        _shoppingCartService = shoppingCartService;
    }
    
    /// <summary>
    /// Gets the shopping basket for the current authenticated customer.
    /// </summary>
    /// <returns>The shopping cart object.</returns>
    /// <response code="200">Returns the shopping cart object.</response>
    /// <response code="401">If the user is not authorized.</response>
    /// <response code="404">If the shopping cart is not found.</response>
    /// <response code="500">If an unexpected error occurs.</response>
    [HttpGet()]
    [ProducesResponseType(typeof(ShoppingCartResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetShoppingBasket()
    {
        try
        {
            var customerId = GetGuidFromClaims();
            var shoppingBasket = await _shoppingCartService.GetActiveShoppingCart(customerId);
            return Ok(shoppingBasket);
        }
        catch (Exception e)
        {
            return HandleException(e);
        }

    }
    
    /// <summary>
    /// Adds a quantity of a product to the shopping cart
    /// </summary>
    /// <param name="productId">The id of the product</param>
    /// <param name="quantity">The wished quantity of the product</param>
    /// <returns>nothing</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     PUT /api/shoppingcart/items/509a4a74-9bec-42e4-8b0a-7cdf2b0ab709
    ///     {
    ///         "quantity": 2
    ///     }
    /// </remarks>
    /// <response code="204">If the product was added to the shopping cart.</response>
    /// <response code="401">If the user is not authorized.</response>
    /// <response code="403">If the adding of the product to the shopping cart is not possible.</response>
    /// <response code="404">If the product is not found.</response>
    /// <response code="500">If an unexpected error occurs.</response>
    [HttpPut("items/{productId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> AddProductToShoppingCart(Guid productId, [FromBody] QuantityRequestDto quantityRequest)
    {
        try
        {
            var customerId = GetGuidFromClaims();
            await _shoppingCartService.AddProductToShoppingCart(customerId, productId, quantityRequest.Quantity);
            return NoContent();
        }
        catch (Exception e)
        {
            return HandleException(e);
        }
    }
    
    /// <summary>
    /// Removes a item from the shopping cart
    /// </summary>
    /// <param name="productId">The items product id</param>
    /// <returns>nothing></returns>
    /// <response code="204">If the product was removed from the shopping cart.</response>
    /// <response code="401">If the user is not authorized.</response>
    /// <response code="403">If the removing of the product from the shopping cart is not possible.</response>
    /// <response code="404">If the product is not found.</response>
    /// <response code="500">If an unexpected error occurs.</response>
    [HttpDelete("items/{productId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> RemoveProductFromShoppingBasket(Guid productId)
    {
        try
        {
            var customerId = GetGuidFromClaims();
            await _shoppingCartService.RemoveProductFromShoppingCart(customerId, productId);
            return NoContent();
        }
        catch (Exception e)
        {
            return HandleException(e);
        }
    }
    
    /// <summary>
    /// Changes the state of the shopping cart to checkout and proceeds with the ordering process
    /// </summary>
    /// <returns>nothing</returns>
    /// <response code="204">If the shopping cart was checked out.</response>
    /// <response code="401">If the user is not authorized.</response>
    /// <response code="404">If the shopping cart is not found.</response>
    /// <response code="500">If an unexpected error occurs.</response>
    [HttpPatch("state/checkout")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<ActionResult> CheckoutShoppingCart([FromBody] CheckoutRequestDto checkoutRequestDto)
    {
        try
        {
            var customerId = GetGuidFromClaims();
            await _shoppingCartService.Checkout(customerId, checkoutRequestDto);
            return NoContent();
        }
        catch (Exception e)
        {
            return HandleException(e);
        }

    }
    
    private Guid GetGuidFromClaims()
    {
        var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(customerId))
        {
            throw new UnauthorizedException("User not authorized.");
        }
        return Guid.Parse(customerId);
    }

    private ActionResult HandleException(Exception exception)
    {
        return exception switch
        {
            EntityNotFoundException => NotFound(exception.Message),
            UnauthorizedException => Unauthorized(exception.Message),
            ShoppingCartDomainException => Forbid(exception.Message),
            _ => StatusCode(500)
        };
    }
}