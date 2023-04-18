using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ShoppingCart.Core.Events;
using ShoppingCart.Core.Product;

namespace ShoppingCart.Application.EventConsumer;

public class ProductRemovedByAdminEventConsumer : INotificationHandler<ProductRemovedByAdminEvent>
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<ProductRemovedByAdminEventConsumer> _logger;
    
    public ProductRemovedByAdminEventConsumer(IServiceScopeFactory serviceScopeFactory, ILogger<ProductRemovedByAdminEventConsumer> logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
    }
    
    public Task Handle(ProductRemovedByAdminEvent notification, CancellationToken cancellationToken)
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            try
            {
                var productRepository = scope.ServiceProvider.GetRequiredService<IProductRepository>();
                productRepository.Delete(notification.ProductId);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while deleting product");
            }

            return Task.CompletedTask;
        }

    }
}