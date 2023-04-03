using MediatR;
using Microsoft.Extensions.DependencyInjection;
using ShoppingCart.Core.Events;
using ShoppingCart.Core.Product;

namespace ShoppingCart.Application.EventConsumer;

public class ProductUpdatedByAdminEventConsumer : INotificationHandler<ProductUpdatedByAdminEvent>
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    
    public ProductUpdatedByAdminEventConsumer(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }
    
    public async Task Handle(ProductUpdatedByAdminEvent notification, CancellationToken cancellationToken)
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var productRepository = scope.ServiceProvider.GetRequiredService<IProductRepository>();
            var product = productRepository.GetById(notification.Id);
            product.Update(notification.Name, notification.Description, notification.Price, notification.Stock);
            productRepository.Update(product);
        }

    }
}