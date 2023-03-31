using Ecommerce.Common.Kafka;
using MediatR;
using ShoppingCart.Core.Events;

namespace ShoppingCart.Application.EventHandlers;

public class CustomerChangedProductQuantityInCartEventHandler : INotificationHandler<CustomerChangedProductQuantityInCartEvent>
{
    private readonly KafkaProducer _kafkaProducer;
    
    public CustomerChangedProductQuantityInCartEventHandler(KafkaProducer kafkaProducer)
    {
        _kafkaProducer = kafkaProducer;
    }
    
    public async Task Handle(CustomerChangedProductQuantityInCartEvent notification, CancellationToken cancellationToken)
    {
        _kafkaProducer.Publish("shopping-cart", "customer-changed-product-quantity-in-cart", notification);
    }
}