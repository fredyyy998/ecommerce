using MediatR;
using Microsoft.Extensions.DependencyInjection;
using ShoppingCart.Core.Events;
using ShoppingCart.Core.Product;

namespace ShoppingCart.Application.EventConsumer;

public class ProductAddedByAdminEventConsumer : INotificationHandler<ProductAddedByAdminEvent>
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    
    public ProductAddedByAdminEventConsumer(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }


    public Task Handle(ProductAddedByAdminEvent notification, CancellationToken cancellationToken)
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var productRepository = scope.ServiceProvider.GetRequiredService<IProductRepository>();
            var product = Product.Create(notification.Id, notification.Name, notification.Description, notification.Price,
                notification.Stock);
            productRepository.Create(product);
            return Task.CompletedTask;
        }
    }
}