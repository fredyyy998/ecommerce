using Ecommerce.Common.Kafka;
using Inventory.Core.DomainEvents;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Inventory.Application.EventHandlers;

public class ProductAddedByAdminEventHandler : INotificationHandler<ProductAddedByAdminEvent>
{
    private readonly KafkaProducer _kafkaProducer;
    private readonly ILogger<ProductAddedByAdminEventHandler> _logger;
    
    public ProductAddedByAdminEventHandler(KafkaProducer kafkaProducer, ILogger<ProductAddedByAdminEventHandler> logger)
    {
        _kafkaProducer = kafkaProducer;
        _logger = logger;
    }
    
    public async Task Handle(ProductAddedByAdminEvent notification, CancellationToken cancellationToken)
    {
        _kafkaProducer.Publish<ProductAddedByAdminEvent>("inventory", "product-added-by-admin", notification);
        _logger.LogInformation($"ProductAddedByAdminEvent published to Kafka: {notification}");
    }
}