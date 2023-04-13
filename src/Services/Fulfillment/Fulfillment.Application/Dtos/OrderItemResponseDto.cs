using Fulfillment.Core.Order;

namespace Fulfillment.Application.Dtos;

public record OrderItemResponseDto(
    Guid ProductId,
    string Name,
    int Quantity,
    Price TotalPrice);