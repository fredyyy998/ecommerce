using Inventory.Core.DomainEvents;
using Inventory.Core.Product;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Inventory.Application.EventConsumer;

public class OrderCancelledEventConsumer : INotificationHandler<OrderCancelledEvent>
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    
    public OrderCancelledEventConsumer(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }
    
    public Task Handle(OrderCancelledEvent notification, CancellationToken cancellationToken)
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var productRepository = scope.ServiceProvider.GetRequiredService<IProductRepository>();
            foreach (var item in notification.Items)
            {
                var product = productRepository.GetById(item.ProductId);
                product.AddStock(item.Quantity);
                productRepository.Update(product);
            }
            return Task.CompletedTask;
        }
    }
}