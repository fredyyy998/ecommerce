using Ecommerce.Common.Kafka;
using MediatR;
using Microsoft.Extensions.Logging;
using ShoppingCart.Core.Events;

namespace ShoppingCart.Application.EventHandlers;

public class CustomerOrderedShoppingCartEventHandler : INotificationHandler<CustomerOrderedShoppingCartEvent>
{
    private readonly KafkaProducer _kafkaProducer;
    private readonly ILogger<CustomerOrderedShoppingCartEventHandler> _logger;
    
    public CustomerOrderedShoppingCartEventHandler(KafkaProducer kafkaProducer, ILogger<CustomerOrderedShoppingCartEventHandler> logger)
    {
        _kafkaProducer = kafkaProducer;
        _logger = logger;
    }
    
    public async Task Handle(CustomerOrderedShoppingCartEvent notification, CancellationToken cancellationToken)
    {
        _kafkaProducer.Publish("shopping-cart", "customer-ordered-shopping-cart", notification);
        _logger.LogInformation($"Published customer-ordered-shopping-cart event {notification}");
    }
}