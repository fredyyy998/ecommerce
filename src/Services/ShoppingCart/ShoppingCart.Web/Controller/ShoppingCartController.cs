
using Inventory.Application.Exceptions;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Application.Dtos;
using ShoppingCart.Application.Services;

namespace ShoppingCart.Web;

[ApiController]
[Route("/api/[controller]")]
public class ShoppingBasketController : Controller
{
    private readonly IShoppingCartService _shoppingCartService;
    
    public ShoppingBasketController(IShoppingCartService shoppingCartService)
    {
        _shoppingCartService = shoppingCartService;
    }
    
    [HttpGet("{customerId:guid}")]
    public async Task<ActionResult> GetShoppingBasket(Guid customerId)
    {
        var shoppingBasket = await _shoppingCartService.GetActiveShoppingCart(customerId);
        return Ok(shoppingBasket);
    }
    
    [HttpPut("{customerId:guid}")]
    public async Task<ActionResult> AddProductToShoppingBasket(Guid customerId, [FromBody] AddItemToShoppingCartRequestDto request)
    {
        try
        {
            await _shoppingCartService.AddProductToShoppingCart(customerId, request.ProductId, request.Quantity);
            return Ok();
        }
        catch (Exception e)
        {
            return HandleException(e);
        }
    }
    
    [HttpDelete("{customerId:guid}")]
    public async Task<ActionResult> RemoveProductFromShoppingBasket(Guid customerId, [FromBody] RemoveItemFromShoppingCartRequestDto request)
    {
        try
        {
            await _shoppingCartService.RemoveProductFromShoppingCart(customerId, request.ProductId, request.Quantity);
            return Ok();
        }
        catch (Exception e)
        {
            return HandleException(e);
        }
    }
    
    [HttpPatch("{customerId:guid}")]
    public async Task<ActionResult> CheckoutShoppingBasket(Guid customerId)
    {
        await _shoppingCartService.Checkout(customerId);
        return Ok();
    }

    private ActionResult HandleException(Exception exception)
    {
        return exception switch
        {
            EntityNotFoundException => NotFound(exception.Message),
            _ => StatusCode(500)
        };
    }
}