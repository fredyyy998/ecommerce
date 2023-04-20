using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ShoppingCart.Core.Events;
using ShoppingCart.Core.Product;

namespace ShoppingCart.Application.EventConsumer;

public class ProductStockUpdateByAdminEventConsumer : INotificationHandler<ProductStockUpdatedByAdminEvent>
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<ProductStockUpdateByAdminEventConsumer> _logger;
    
    public ProductStockUpdateByAdminEventConsumer(IServiceScopeFactory serviceScopeFactory, ILogger<ProductStockUpdateByAdminEventConsumer> logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
    }
    
    public async Task Handle(ProductStockUpdatedByAdminEvent notification, CancellationToken cancellationToken)
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            try
            {
                var productRepository =  scope.ServiceProvider.GetRequiredService<IProductRepository>();
                var product = await productRepository.GetById(notification.ProductId);
                product.Update(product.Name, product.Description, product.Price, notification.Stock);
                productRepository.Update(product);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while updating product stock");
            }
        }
    }
}