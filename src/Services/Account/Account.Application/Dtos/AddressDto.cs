namespace Account.Application.Dtos;

public record AddressDto(
    string Street,
    string City,
    string Zip,
    string Country);