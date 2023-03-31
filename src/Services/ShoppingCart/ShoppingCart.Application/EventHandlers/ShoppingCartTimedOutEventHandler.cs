using Ecommerce.Common.Kafka;
using MediatR;
using ShoppingCart.Core.Events;

namespace ShoppingCart.Application.EventHandlers;

public class ShoppingCartTimedOutEventHandler : INotificationHandler<ShoppingCartTimedOutEvent>
{
    private readonly KafkaProducer _kafkaProducer;
    
    public ShoppingCartTimedOutEventHandler(KafkaProducer kafkaProducer)
    {
        _kafkaProducer = kafkaProducer;
    }
    
    public async Task Handle(ShoppingCartTimedOutEvent notification, CancellationToken cancellationToken)
    {
        _kafkaProducer.Publish("shopping-cart", "shopping-cart-timed-out", notification);
    }
}