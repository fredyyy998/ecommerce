using MediatR;
using Microsoft.Extensions.DependencyInjection;
using ShoppingCart.Core.Events;
using ShoppingCart.Core.Product;

namespace ShoppingCart.Application.EventConsumer;

public class ProductRemovedByAdminEventConsumer : INotificationHandler<ProductRemovedByAdminEvent>
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    
    public ProductRemovedByAdminEventConsumer(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }
    
    public Task Handle(ProductRemovedByAdminEvent notification, CancellationToken cancellationToken)
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var productRepository = scope.ServiceProvider.GetRequiredService<IProductRepository>();
            productRepository.Delete(notification.ProductId);
            return Task.CompletedTask;
        }

    }
}