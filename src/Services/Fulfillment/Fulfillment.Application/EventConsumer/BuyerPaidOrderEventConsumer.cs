using Fulfillment.Core.DomainEvents;
using Fulfillment.Core.Order;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Fulfillment.Application.EventConsumer;

public class BuyerPaidOrderEventConsumer : INotificationHandler<BuyerPaidOrderEvent>
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<BuyerPaidOrderEventConsumer> _logger;

    public BuyerPaidOrderEventConsumer(IServiceScopeFactory serviceScopeFactory, ILogger<BuyerPaidOrderEventConsumer> logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
    }
    
    public async Task Handle(BuyerPaidOrderEvent notification, CancellationToken cancellationToken)
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            try
            {
                var orderRepository = scope.ServiceProvider.GetRequiredService<IOrderRepository>();
                var order = await orderRepository.FindByIdAsync(notification.OrderId);
                order.ChangeState(OrderState.Paid);
                await orderRepository.UpdateAsync(order);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while handling BuyerPaidOrderEvent");
            }

        }
    }
}