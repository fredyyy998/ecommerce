using AutoMapper;
using Fulfillment.Application.Dtos;
using Fulfillment.Application.Exceptions;
using Fulfillment.Core.Order;

namespace Fulfillment.Application.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    
    private readonly IMapper _mapper;
    
    public OrderService(IOrderRepository orderRepository, IMapper mapper)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
    }
    
    public async Task<OrderResponseDto> GetOrder(Guid orderId)
    {
        var orderResult = await _orderRepository.FindByIdAsync(orderId);
        return _mapper.Map<OrderResponseDto>(orderResult);
    }

    public async Task<IEnumerable<OrderResponseDto>> GetOrdersByBuyer(Guid buyerId)
    {
        var ordersResult = await _orderRepository.FindAllByBuyerIdAsync(buyerId);
        return _mapper.Map<IEnumerable<OrderResponseDto>>(ordersResult);
    }

    public async Task PayOrder(Guid orderId)
    {
        await ChangeState(orderId, OrderState.Paid);
    }

    public async Task ShipOrder(Guid orderId)
    {
        await ChangeState(orderId, OrderState.Shipped);
    }

    public async Task DeliverOrder(Guid orderId)
    {
        await ChangeState(orderId, OrderState.Delivered);
    }

    public async Task CancelOrder(Guid orderId)
    {
        await ChangeState(orderId, OrderState.Cancelled);
    }

    private async Task ChangeState(Guid orderId, OrderState state)
    {
        var orderResult = await _orderRepository.FindByIdAsync(orderId);
        if (orderResult == null)
        {
            throw new EntityNotFoundException($"Order with {orderId} could not be found");
        }

        orderResult.ChangeState(state);
    }
}