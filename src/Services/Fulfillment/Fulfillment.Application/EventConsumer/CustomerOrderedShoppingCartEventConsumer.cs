using Fulfillment.Core.DomainEvents;
using Fulfillment.Core.Order;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Fulfillment.Application.EventConsumer;

public class CustomerOrderedShoppingCartEventConsumer : INotificationHandler<CustomerOrderedShoppingCartEvent>
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    
    public CustomerOrderedShoppingCartEventConsumer(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }
    
    public async Task Handle(CustomerOrderedShoppingCartEvent notification, CancellationToken cancellationToken)
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var orderRepository = scope.ServiceProvider.GetRequiredService<IOrderRepository>();
            var order = Order.Create(notification.CustomerId);
            foreach (var item in notification.Items)
            {
                order.AddOrderItem(item);  
            }
            
            orderRepository.SaveAsync(order);
        }
    }
}