using Ecommerce.Common.Kafka;
using Fulfillment.Core.DomainEvents;
using MediatR;

namespace Fulfillment.Application.EventHandler;

public class AdministratorShippedOrderEventHandler : INotificationHandler<AdministratorShippedOrderEvent>
{
    private readonly KafkaProducer _kafkaProducer;
    
    public Task Handle(AdministratorShippedOrderEvent notification, CancellationToken cancellationToken)
    {
        _kafkaProducer.Publish("fulfillment", "administrator-shipped-order", notification);
        return Task.CompletedTask;
    }
}