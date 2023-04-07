using Fulfillment.Core.Buyer;

namespace Fulfillment.Application.Dtos;

public record BuyerResponseDto(
    Guid Id,
    PersonalInformation PersonalInformation,
    Address ShippingAddress,
    PaymentInformation PaymentInformation);