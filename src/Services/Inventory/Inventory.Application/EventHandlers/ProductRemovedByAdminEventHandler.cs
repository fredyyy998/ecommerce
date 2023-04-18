using Ecommerce.Common.Kafka;
using Inventory.Core.DomainEvents;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Inventory.Application.EventHandlers;

public class ProductRemovedByAdminEventHandler : INotificationHandler<ProductRemovedByAdmin>
{
    private readonly KafkaProducer _kafkaProducer;
    private readonly ILogger<ProductRemovedByAdminEventHandler> _logger;
    
    public ProductRemovedByAdminEventHandler(KafkaProducer kafkaProducer, ILogger<ProductRemovedByAdminEventHandler> logger)
    {
        _kafkaProducer = kafkaProducer;
        _logger = logger;
    }
    
    public async Task Handle(ProductRemovedByAdmin notification, CancellationToken cancellationToken)
    {
        _kafkaProducer.Publish<ProductRemovedByAdmin>("inventory", "product-removed-by-admin", notification);
        _logger.LogInformation($"ProductRemovedByAdmin published to Kafka: {notification}");
    }
}