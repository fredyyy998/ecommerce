using Ecommerce.Common.Kafka;
using Inventory.Core.DomainEvents;
using MediatR;

namespace Inventory.Application.EventHandlers;

public class ProductUpdatedByAdminEventHandler : INotificationHandler<ProductUpdatedByAdmin>
{
    private readonly KafkaProducer _kafkaProducer;
    
    public ProductUpdatedByAdminEventHandler(KafkaProducer kafkaProducer)
    {
        _kafkaProducer = kafkaProducer;
    }
    
    public async Task Handle(ProductUpdatedByAdmin notification, CancellationToken cancellationToken)
    {
        _kafkaProducer.Publish<ProductUpdatedByAdmin>("inventory", "product-updated-by-admin", notification);
    }
}