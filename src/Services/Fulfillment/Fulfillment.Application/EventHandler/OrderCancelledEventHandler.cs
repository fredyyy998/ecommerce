using Ecommerce.Common.Kafka;
using Fulfillment.Core.DomainEvents;
using MediatR;

namespace Fulfillment.Application.EventHandler;

public class OrderCancelledEventHandler : INotificationHandler<OrderCancelledEvent>
{
    private readonly KafkaProducer _kafkaProducer;
    
    public OrderCancelledEventHandler(KafkaProducer kafkaProducer)
    {
        _kafkaProducer = kafkaProducer;
    }
    
    public Task Handle(OrderCancelledEvent notification, CancellationToken cancellationToken)
    {
        _kafkaProducer.Publish("fulfillment", "order-cancelled", notification);
        return Task.CompletedTask;
    }
}