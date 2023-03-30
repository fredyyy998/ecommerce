using Ecommerce.Common.Kafka;
using Inventory.Core.DomainEvents;
using MediatR;

namespace Inventory.Application.EventHandlers;

public class ProductUpdatedByAdminEventHandler : INotificationHandler<ProductUpdatedByAdminEvent>
{
    private readonly KafkaProducer _kafkaProducer;
    
    public ProductUpdatedByAdminEventHandler(KafkaProducer kafkaProducer)
    {
        _kafkaProducer = kafkaProducer;
    }
    
    public async Task Handle(ProductUpdatedByAdminEvent notification, CancellationToken cancellationToken)
    {
        _kafkaProducer.Publish<ProductUpdatedByAdminEvent>("inventory", "product-updated-by-admin", notification);
    }
}