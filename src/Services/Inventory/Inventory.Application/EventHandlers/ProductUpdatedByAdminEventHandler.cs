using Ecommerce.Common.Kafka;
using Inventory.Core.DomainEvents;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Inventory.Application.EventHandlers;

public class ProductUpdatedByAdminEventHandler : INotificationHandler<ProductUpdatedByAdminEvent>
{
    private readonly KafkaProducer _kafkaProducer;
    private readonly ILogger<ProductUpdatedByAdminEventHandler> _logger;
    
    public ProductUpdatedByAdminEventHandler(KafkaProducer kafkaProducer, ILogger<ProductUpdatedByAdminEventHandler> logger)
    {
        _kafkaProducer = kafkaProducer;
        _logger = logger;
    }
    
    public async Task Handle(ProductUpdatedByAdminEvent notification, CancellationToken cancellationToken)
    {
        _kafkaProducer.Publish<ProductUpdatedByAdminEvent>("inventory", "product-updated-by-admin", notification);
        _logger.LogInformation($"ProductUpdatedByAdminEvent published to Kafka: {notification}");
    }
}