using Ecommerce.Common.Kafka;
using Inventory.Core.DomainEvents;
using MediatR;

namespace Inventory.Application.EventHandlers;

public class ProductRemovedByAdminEventHandler : INotificationHandler<ProductRemovedByAdmin>
{
    private readonly KafkaProducer _kafkaProducer;
    
    public ProductRemovedByAdminEventHandler(KafkaProducer kafkaProducer)
    {
        _kafkaProducer = kafkaProducer;
    }
    
    public async Task Handle(ProductRemovedByAdmin notification, CancellationToken cancellationToken)
    {
        _kafkaProducer.Publish<ProductRemovedByAdmin>("inventory", "product-removed-by-admin", notification);
    }
}