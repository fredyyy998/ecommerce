namespace Inventory.Application.Dtos;

public record ProductUpdateDto(
    string Name,
    string Description,
    decimal GrossPrice,
    List<KeyValuePair<string, string>> ProductInformation);