using Ecommerce.Common.Kafka;
using Inventory.Core.DomainEvents;
using MediatR;

namespace Inventory.Application.EventHandlers;

public class ProductAddedByAdminEventHandler : INotificationHandler<ProductAddedByAdmin>
{
    private readonly KafkaProducer _kafkaProducer;
    
    public ProductAddedByAdminEventHandler(KafkaProducer kafkaProducer)
    {
        _kafkaProducer = kafkaProducer;
    }
    
    public async Task Handle(ProductAddedByAdmin notification, CancellationToken cancellationToken)
    {
        _kafkaProducer.Publish<ProductAddedByAdmin>("inventory", "product-added-by-admin", notification);
    }
}