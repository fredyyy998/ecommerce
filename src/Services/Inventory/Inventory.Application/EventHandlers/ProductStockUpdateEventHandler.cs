using AutoMapper;
using Ecommerce.Common.Kafka;
using Inventory.Core.DomainEvents;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Inventory.Application.EventHandlers;

public class ProductStockUpdateEventHandler : INotificationHandler<ProductStockUpdated>
{
    private readonly KafkaProducer _kafkaProducer;
    private readonly ILogger<ProductStockUpdateEventHandler> _logger;

    public ProductStockUpdateEventHandler(KafkaProducer kafkaProducer, ILogger<ProductStockUpdateEventHandler> logger)
    {
        _kafkaProducer = kafkaProducer;
        _logger = logger;
    }
    
    public async Task Handle(ProductStockUpdated notification, CancellationToken cancellationToken)
    {
        _kafkaProducer.Publish<ProductStockUpdated>("inventory", "product-stock-updated", notification);
        _logger.LogInformation($"ProductStockUpdated published to Kafka: {notification}");
    }
}