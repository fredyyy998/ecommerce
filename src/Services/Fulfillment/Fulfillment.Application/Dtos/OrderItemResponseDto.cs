using Fulfillment.Core.Order;

namespace Fulfillment.Application.Dtos;

public record OrderItemResponseDto(
    Guid ProductId,
    string ProductName,
    int Quantity,
    Price TotalPrice);