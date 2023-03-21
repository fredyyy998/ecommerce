namespace Inventory.Application.Dtos;

public record ProductResponseDto(
    Guid Id,
    string Name,
    string Description,
    PriceDto Price);