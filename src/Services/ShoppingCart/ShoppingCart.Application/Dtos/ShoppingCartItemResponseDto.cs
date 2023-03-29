namespace ShoppingCart.Application.Dtos;

public record ShoppingCartItemResponseDto(
    Guid Id,
    string Name,
    string Description,
    decimal Price,
    string CurrencyCode,
    int Quantity);