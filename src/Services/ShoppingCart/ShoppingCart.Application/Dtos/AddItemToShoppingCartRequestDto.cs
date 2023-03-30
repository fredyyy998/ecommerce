namespace ShoppingCart.Application.Dtos;

public record AddItemToShoppingCartRequestDto(
    Guid ProductId,
    int Quantity);