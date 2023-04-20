using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ShoppingCart.Core.Events;
using ShoppingCart.Core.Product;

namespace ShoppingCart.Application.EventConsumer;

public class ProductUpdatedByAdminEventConsumer : INotificationHandler<ProductUpdatedByAdminEvent>
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<ProductUpdatedByAdminEventConsumer> _logger;
    
    public ProductUpdatedByAdminEventConsumer(IServiceScopeFactory serviceScopeFactory, ILogger<ProductUpdatedByAdminEventConsumer> logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
    }
    
    public async Task Handle(ProductUpdatedByAdminEvent notification, CancellationToken cancellationToken)
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            try
            {
                var productRepository = scope.ServiceProvider.GetRequiredService<IProductRepository>();
                var product = await productRepository.GetById(notification.Id);
                product.Update(notification.Name, notification.Description, notification.Price, notification.Stock);
                productRepository.Update(product);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while updating product");
            }
        }

    }
}