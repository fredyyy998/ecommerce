namespace ShoppingCart.Application.Dtos;

public record ShoppingCartItemResponseDto(
    Guid ProductId,
    string Name,
    string Description,
    decimal Price,
    string CurrencyCode,
    int Quantity);