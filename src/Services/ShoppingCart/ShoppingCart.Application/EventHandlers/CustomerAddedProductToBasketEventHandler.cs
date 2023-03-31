using Ecommerce.Common.Kafka;
using MediatR;
using ShoppingCart.Core.Events;

namespace ShoppingCart.Application.EventHandlers;

public class CustomerAddedProductToBasketEventHandler : INotificationHandler<CustomerAddedProductToBasketEvent>
{
    private readonly KafkaProducer _kafkaProducer;
    
    public CustomerAddedProductToBasketEventHandler(KafkaProducer kafkaProducer)
    {
        _kafkaProducer = kafkaProducer;
    }
    
    public async Task Handle(CustomerAddedProductToBasketEvent notification, CancellationToken cancellationToken)
    {
        _kafkaProducer.Publish("shopping-cart", "customer-added-product-to-basket", notification);
    }
}