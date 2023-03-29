namespace ShoppingCart.Application.Dtos;

public record ShoppingCartResponseDto(
    Guid Id,
    IEnumerable<ShoppingCartItemResponseDto> Items);