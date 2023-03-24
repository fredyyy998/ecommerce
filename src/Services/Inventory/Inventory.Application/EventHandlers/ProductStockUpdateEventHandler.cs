using AutoMapper;
using Ecommerce.Common.Kafka;
using Inventory.Core.DomainEvents;
using MediatR;

namespace Inventory.Application.EventHandlers;

public class ProductStockUpdateEventHandler : INotificationHandler<ProductStockUpdated>
{
    private readonly KafkaProducer _kafkaProducer;

    public ProductStockUpdateEventHandler(KafkaProducer kafkaProducer)
    {
        _kafkaProducer = kafkaProducer;
    }
    
    public async Task Handle(ProductStockUpdated notification, CancellationToken cancellationToken)
    {
        _kafkaProducer.Publish<ProductStockUpdated>("inventory", "product-stock-updated", notification);
    }
}