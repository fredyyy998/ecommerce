using Fulfillment.Application.Dtos;
using Fulfillment.Core.Buyer;
using Fulfillment.Core.Order;

namespace Fulfillment.Application.Services;

public interface IOrderService
{
    Task<OrderResponseDto> GetOrder(Guid orderId);
    Task<IEnumerable<OrderResponseDto>> GetOrdersByBuyer(Guid buyerId);
    Task PayOrder(Guid orderId);
    Task ShipOrder(Guid orderId);
    Task DeliverOrder(Guid orderId);
    Task CancelOrder(Guid orderId);
    Task SubmitOrder(Guid orderId, SubmitOrderRequestDto submitOrderRequestDto);
}

