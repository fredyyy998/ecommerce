using Fulfillment.Core.DomainEvents;
using Fulfillment.Core.Order;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Fulfillment.Application.EventConsumer;

public class BuyerPaidOrderEventConsumer : INotificationHandler<BuyerPaidOrderEvent>
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    
    public BuyerPaidOrderEventConsumer(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }
    
    public async Task Handle(BuyerPaidOrderEvent notification, CancellationToken cancellationToken)
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var orderRepository = scope.ServiceProvider.GetRequiredService<IOrderRepository>();
            var order = await orderRepository.FindByIdAsync(notification.OrderId);
            order.ChangeState(OrderState.Paid);
            orderRepository.UpdateAsync(order);
        }
    }
}