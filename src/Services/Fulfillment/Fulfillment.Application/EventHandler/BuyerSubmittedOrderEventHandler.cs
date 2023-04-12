using Ecommerce.Common.Kafka;
using Fulfillment.Core.DomainEvents;
using MediatR;

namespace Fulfillment.Application.EventHandler;

public class BuyerSubmittedOrderEventHandler : INotificationHandler<BuyerSubmittedOrderEvent>
{
    private readonly KafkaProducer _kafkaProducer;
    
    public BuyerSubmittedOrderEventHandler(KafkaProducer kafkaProducer)
    {
        _kafkaProducer = kafkaProducer;
    }
    
    public Task Handle(BuyerSubmittedOrderEvent notification, CancellationToken cancellationToken)
    {
        _kafkaProducer.Publish("fulfillment", "buyer-submitted-order", notification);
        return Task.CompletedTask;
    }
}