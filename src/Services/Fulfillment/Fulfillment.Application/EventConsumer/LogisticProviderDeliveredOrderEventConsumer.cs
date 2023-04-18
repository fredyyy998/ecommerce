using Fulfillment.Core.DomainEvents;
using Fulfillment.Core.Order;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Fulfillment.Application.EventConsumer;

public class LogisticProviderDeliveredOrderEventConsumer : INotificationHandler<LogisticProviderDeliveredOrderEvent>
{
    private IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<LogisticProviderDeliveredOrderEventConsumer> _logger;
    
    public LogisticProviderDeliveredOrderEventConsumer(IServiceScopeFactory serviceScopeFactory, ILogger<LogisticProviderDeliveredOrderEventConsumer> logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
    }
    
    public async Task Handle(LogisticProviderDeliveredOrderEvent notification, CancellationToken cancellationToken)
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            try
            {
                var orderRepository = scope.ServiceProvider.GetRequiredService<IOrderRepository>();
                var order = await orderRepository.FindByIdAsync(notification.OrderId);
                order.ChangeState(OrderState.Delivered);
                await orderRepository.UpdateAsync(order);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while handling LogisticProviderDeliveredOrderEvent");
            }
        }
    }
}