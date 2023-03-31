namespace ShoppingCart.Application.Dtos;

public record RemoveItemFromShoppingCartRequestDto(
    Guid ProductId,
    int Quantity);