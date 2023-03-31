
using System.Security.Claims;
using Account.Application.Exceptions;
using Inventory.Application.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Application.Dtos;
using ShoppingCart.Application.Services;

namespace ShoppingCart.Web;

[ApiController]
[Authorize]
[Route("/api/[controller]")]
public class ShoppingBasketController : Controller
{
    private readonly IShoppingCartService _shoppingCartService;
    
    public ShoppingBasketController(IShoppingCartService shoppingCartService)
    {
        _shoppingCartService = shoppingCartService;
    }
    
    [HttpGet()]
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
    
    [HttpPut()]
    public async Task<ActionResult> AddProductToShoppingBasket([FromBody] AddItemToShoppingCartRequestDto request)
    {
        try
        {
            var customerId = GetGuidFromClaims();
            await _shoppingCartService.AddProductToShoppingCart(customerId, request.ProductId, request.Quantity);
            return Ok();
        }
        catch (Exception e)
        {
            return HandleException(e);
        }
    }
    
    [HttpDelete()]
    public async Task<ActionResult> RemoveProductFromShoppingBasket([FromBody] RemoveItemFromShoppingCartRequestDto request)
    {
        try
        {
            var customerId = GetGuidFromClaims();
            await _shoppingCartService.RemoveProductFromShoppingCart(customerId, request.ProductId, request.Quantity);
            return Ok();
        }
        catch (Exception e)
        {
            return HandleException(e);
        }
    }
    
    [HttpPatch()]
    public async Task<ActionResult> CheckoutShoppingBasket()
    {
        try
        {
            var customerId = GetGuidFromClaims();
            await _shoppingCartService.Checkout(customerId);
            return Ok();
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
            _ => StatusCode(500)
        };
    }
}