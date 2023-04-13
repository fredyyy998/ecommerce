namespace Fulfillment.Core.DomainEvents;

public record ShoppingCartItemDto(
    Guid ProductId,
    string Name,
    decimal NetPrice,
    decimal GrossPrice,
    string CurrencyCode,
    int Quantity,
    decimal TotalPrice
);