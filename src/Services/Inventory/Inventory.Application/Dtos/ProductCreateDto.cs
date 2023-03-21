namespace Inventory.Application.Dtos;

public record ProductCreateDto(
    string Name,
    string Description,
    decimal GrossPrice);