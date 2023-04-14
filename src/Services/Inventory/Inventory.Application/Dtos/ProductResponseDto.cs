using Inventory.Core.Product;

namespace Inventory.Application.Dtos;

public record ProductResponseDto(
    Guid Id,
    string Name,
    string Description,
    PriceDto Price, 
    IReadOnlyCollection<ProductInformation> Informations);