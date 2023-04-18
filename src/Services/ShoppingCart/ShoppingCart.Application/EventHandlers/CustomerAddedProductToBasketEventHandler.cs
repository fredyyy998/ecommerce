using Ecommerce.Common.Kafka;
using MediatR;
using Microsoft.Extensions.Logging;
using ShoppingCart.Core.Events;

namespace ShoppingCart.Application.EventHandlers;

public class CustomerAddedProductToBasketEventHandler : INotificationHandler<CustomerAddedProductToBasketEvent>
{
    private readonly KafkaProducer _kafkaProducer;
    private readonly ILogger<CustomerAddedProductToBasketEventHandler> _logger;
    
    public CustomerAddedProductToBasketEventHandler(KafkaProducer kafkaProducer, ILogger<CustomerAddedProductToBasketEventHandler> logger)
    {
        _kafkaProducer = kafkaProducer;
        _logger = logger;
    }
    
    public async Task Handle(CustomerAddedProductToBasketEvent notification, CancellationToken cancellationToken)
    {
        _kafkaProducer.Publish("shopping-cart", "customer-added-product-to-basket", notification);
        _logger.LogInformation($"Published customer-added-product-to-basket event {notification}");
    }
}