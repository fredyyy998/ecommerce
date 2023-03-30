using Ecommerce.Common.Kafka;
using Inventory.Core.DomainEvents;
using MediatR;

namespace Inventory.Application.EventHandlers;

public class ProductAddedByAdminEventHandler : INotificationHandler<ProductAddedByAdminEvent>
{
    private readonly KafkaProducer _kafkaProducer;
    
    public ProductAddedByAdminEventHandler(KafkaProducer kafkaProducer)
    {
        _kafkaProducer = kafkaProducer;
    }
    
    public async Task Handle(ProductAddedByAdminEvent notification, CancellationToken cancellationToken)
    {
        _kafkaProducer.Publish<ProductAddedByAdminEvent>("inventory", "product-added-by-admin", notification);
    }
}