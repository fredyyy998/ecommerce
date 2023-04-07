using Fulfillment.Core.Order;

namespace Fulfillment.Application.Dtos;

public record OrderResponseDto(
    Guid Id,
    Guid BuyerId,
    IReadOnlyCollection<OrderItemResponseDto> Products,
    Price TotalPrice,
    DateTime OrderDate,
    OrderState State);

