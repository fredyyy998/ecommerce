using Inventory.Core.Product;

namespace Inventory.Application.Dtos;

public record AdminProductResponseDto(
    Guid Id,
    string Name,
    string Description,
    PriceDto Price,
    int Stock,
    IReadOnlyCollection<ProductInformation> Information
);