using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ShoppingCart.Core.Events;
using ShoppingCart.Core.Product;

namespace ShoppingCart.Application.EventConsumer;

public class ProductAddedByAdminEventConsumer : INotificationHandler<ProductAddedByAdminEvent>
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<ProductAddedByAdminEventConsumer> _logger;
    
    public ProductAddedByAdminEventConsumer(IServiceScopeFactory serviceScopeFactory, ILogger<ProductAddedByAdminEventConsumer> logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
    }


    public Task Handle(ProductAddedByAdminEvent notification, CancellationToken cancellationToken)
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            try
            {
                var productRepository = scope.ServiceProvider.GetRequiredService<IProductRepository>();
                var product = Product.Create(notification.Id, notification.Name, notification.Description, notification.Price,
                    notification.Stock);
                productRepository.Create(product);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while creating product");
            }
            return Task.CompletedTask;
        }
    }
}