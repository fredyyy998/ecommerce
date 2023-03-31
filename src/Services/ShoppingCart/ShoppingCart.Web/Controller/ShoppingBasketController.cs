
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
        await _shoppingCartService.AddProductToShoppingCart(customerId, request.ProductId, request.Quantity);
        return Ok();
    }
    
    [HttpDelete("{customerId:guid}")]
    public async Task<ActionResult> RemoveProductFromShoppingBasket(Guid customerId, [FromBody] RemoveItemFromShoppingCartRequestDto request)
    {
        await _shoppingCartService.RemoveProductFromShoppingCart(customerId, request.ProductId, request.Quantity);
        return Ok();
    }

}