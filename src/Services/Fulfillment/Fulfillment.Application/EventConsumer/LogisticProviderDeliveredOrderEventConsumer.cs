using Fulfillment.Core.DomainEvents;
using Fulfillment.Core.Order;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Fulfillment.Application.EventConsumer;

public class LogisticProviderDeliveredOrderEventConsumer : INotificationHandler<LogisticProviderDeliveredOrderEvent>
{
    private IServiceScopeFactory _serviceScopeFactory;
    
    public LogisticProviderDeliveredOrderEventConsumer(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }
    
    public async Task Handle(LogisticProviderDeliveredOrderEvent notification, CancellationToken cancellationToken)
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var orderRepository = scope.ServiceProvider.GetRequiredService<IOrderRepository>();
            var order = await orderRepository.FindByIdAsync(notification.OrderId);
            order.ChangeState(OrderState.Delivered);
            orderRepository.UpdateAsync(order);
        }
    }
}