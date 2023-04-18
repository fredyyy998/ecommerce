using Ecommerce.Common.Kafka;
using MediatR;
using Microsoft.Extensions.Logging;
using ShoppingCart.Core.Events;

namespace ShoppingCart.Application.EventHandlers;

public class CustomerChangedProductQuantityInCartEventHandler : INotificationHandler<CustomerChangedProductQuantityInCartEvent>
{
    private readonly KafkaProducer _kafkaProducer;
    private readonly ILogger<CustomerChangedProductQuantityInCartEventHandler> _logger;
    
    public CustomerChangedProductQuantityInCartEventHandler(KafkaProducer kafkaProducer, ILogger<CustomerChangedProductQuantityInCartEventHandler> logger)
    {
        _kafkaProducer = kafkaProducer;
        _logger = logger;
    }
    
    public async Task Handle(CustomerChangedProductQuantityInCartEvent notification, CancellationToken cancellationToken)
    {
        _kafkaProducer.Publish("shopping-cart", "customer-changed-product-quantity-in-cart", notification);
        _logger.LogInformation($"Published customer-changed-product-quantity-in-cart event {notification}");
    }
}