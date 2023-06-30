
namespace Offering.Dtos;

public record CreateOfferRequestDto(
    string name,
    decimal grossPrice,
    decimal taxValue,
    DateTime startDate,
    DateTime endDate
);

public record CreateSingleOfferRequestDto(
    string name,
    decimal grossPrice,
    decimal taxValue, 
    DateTime startDate,
    DateTime endDate,
    Guid productId) : CreateOfferRequestDto(name, grossPrice, taxValue, startDate, endDate);

public record ProductRequestDto(
    string name,
    string description);

public record CreateSingleOfferWithProductRequestDto(
    string name,
    decimal grossPrice,
    decimal taxValue, 
    DateTime startDate,
    DateTime endDate, 
    ProductRequestDto product) : CreateOfferRequestDto(name, grossPrice, taxValue, startDate, endDate);
    
public record CreatePackageOfferRequestDto(
    string name,
    decimal grossPrice,
    decimal taxValue,
    DateTime startDate,
    DateTime endDate,
    List<Guid> productIds) : CreateOfferRequestDto(name, grossPrice, taxValue, startDate, endDate);
    
public record CreatePackageOfferWithProductRequestDto(
    string name,
    decimal grossPrice,
    decimal taxValue,
    DateTime startDate,
    DateTime endDate,
    List<ProductRequestDto> products) : CreateOfferRequestDto(name, grossPrice, taxValue, startDate, endDate);