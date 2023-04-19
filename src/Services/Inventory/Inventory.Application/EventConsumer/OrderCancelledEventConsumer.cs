using Inventory.Core.DomainEvents;
using Inventory.Core.Product;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Inventory.Application.EventConsumer;

public class OrderCancelledEventConsumer : INotificationHandler<OrderCancelledEvent>
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<OrderCancelledEventConsumer> _logger;
    
    public OrderCancelledEventConsumer(IServiceScopeFactory serviceScopeFactory, ILogger<OrderCancelledEventConsumer> logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
    }
    
    public async Task Handle(OrderCancelledEvent notification, CancellationToken cancellationToken)
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            try
            {
                var productRepository = scope.ServiceProvider.GetRequiredService<IProductRepository>();
                foreach (var item in notification.Items)
                {
                    var product = await productRepository.GetById(item.ProductId);
                    product.AddStock(item.Quantity);
                    await productRepository.Update(product);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while handling OrderCancelledEvent");
            }
        }
    }
}