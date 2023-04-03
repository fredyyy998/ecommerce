using MediatR;
using Microsoft.Extensions.DependencyInjection;
using ShoppingCart.Core.Events;
using ShoppingCart.Core.Product;

namespace ShoppingCart.Application.EventConsumer;

public class ProductStockUpdateByAdminEventConsumer : INotificationHandler<ProductStockUpdatedByAdminEvent>
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    
    public ProductStockUpdateByAdminEventConsumer(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }
    
    public async Task Handle(ProductStockUpdatedByAdminEvent notification, CancellationToken cancellationToken)
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var productRepository = scope.ServiceProvider.GetRequiredService<IProductRepository>();
            var product = productRepository.GetById(notification.ProductId);
            product.Update(product.Name, product.Description, product.Price, notification.Stock);
            productRepository.Update(product);
        }
    }
}