using Ecommerce.Common.Kafka;
using MediatR;
using ShoppingCart.Core.Events;

namespace ShoppingCart.Application.EventHandlers;

public class CustomerOrderedShoppingCartEventHandler : INotificationHandler<CustomerOrderedShoppingCartEvent>
{
    private readonly KafkaProducer _kafkaProducer;
    
    public CustomerOrderedShoppingCartEventHandler(KafkaProducer kafkaProducer)
    {
        _kafkaProducer = kafkaProducer;
    }
    
    public async Task Handle(CustomerOrderedShoppingCartEvent notification, CancellationToken cancellationToken)
    {
        _kafkaProducer.Publish("shopping-cart", "customer-ordered-shopping-cart", notification);
    }
}