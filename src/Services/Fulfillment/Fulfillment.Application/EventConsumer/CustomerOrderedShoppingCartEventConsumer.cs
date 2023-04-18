using Fulfillment.Core.DomainEvents;
using Fulfillment.Core.Order;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Fulfillment.Application.EventConsumer;

public class CustomerOrderedShoppingCartEventConsumer : INotificationHandler<CustomerOrderedShoppingCartEvent>
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    private readonly ILogger<CustomerOrderedShoppingCartEventConsumer> _logger;
    public CustomerOrderedShoppingCartEventConsumer(IServiceScopeFactory serviceScopeFactory, ILogger<CustomerOrderedShoppingCartEventConsumer> logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
    }
    
    public async Task Handle(CustomerOrderedShoppingCartEvent notification, CancellationToken cancellationToken)
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            try
            {
                var orderRepository = scope.ServiceProvider.GetRequiredService<IOrderRepository>();
                var order = Order.Create(notification.CustomerId, notification.ShoppingCartCheckout.ShippingAddress);
                foreach (var item in notification.Items)
                {
                    // TODO TAX is hard coded and not given from event
                    order.AddOrderItem(OrderItem.Create(item.ProductId, item.Name, item.GrossPrice, item.NetPrice, item.CurrencyCode, 19, item.Quantity));  
                }
            
                await orderRepository.SaveAsync(order);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while handling CustomerOrderedShoppingCartEvent");
            }
        }
    }
}