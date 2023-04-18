using Inventory.Core.DomainEvents;
using Inventory.Core.Product;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Inventory.Application.EventConsumer;

public class CustomerOrderedShoppingCartEventConsumer : INotificationHandler<CustomerOrderedShoppingCartEvent>
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    
    public CustomerOrderedShoppingCartEventConsumer(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }
    
    public Task Handle(CustomerOrderedShoppingCartEvent notification, CancellationToken cancellationToken)
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var productRepository = scope.ServiceProvider.GetRequiredService<IProductRepository>();
            foreach (var item in notification.Items)
            {
                var product = productRepository.GetById(item.ProductId);
                product.RemoveStock(item.Quantity);
                productRepository.Update(product);
            }
            return Task.CompletedTask;
        }
    }
    
}