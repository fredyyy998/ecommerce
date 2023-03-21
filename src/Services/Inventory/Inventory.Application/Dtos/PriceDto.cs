namespace Inventory.Application.Dtos;

public record PriceDto(
    decimal GrossPrice,
    decimal NetPrice,
    int SalesTax,
    string CurrencyCode);