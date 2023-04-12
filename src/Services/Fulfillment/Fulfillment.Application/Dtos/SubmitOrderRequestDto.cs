
namespace Fulfillment.Application.Dtos;

public record SubmitOrderRequestDto(
    string Street,
    string Zip,
    string City,
    string Country);