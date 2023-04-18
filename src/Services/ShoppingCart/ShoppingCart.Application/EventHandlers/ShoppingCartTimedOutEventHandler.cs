using Ecommerce.Common.Kafka;
using MediatR;
using Microsoft.Extensions.Logging;
using ShoppingCart.Core.Events;

namespace ShoppingCart.Application.EventHandlers;

public class ShoppingCartTimedOutEventHandler : INotificationHandler<ShoppingCartTimedOutEvent>
{
    private readonly KafkaProducer _kafkaProducer;
    private readonly ILogger<ShoppingCartTimedOutEventHandler> _logger;
    
    public ShoppingCartTimedOutEventHandler(KafkaProducer kafkaProducer, ILogger<ShoppingCartTimedOutEventHandler> logger)
    {
        _kafkaProducer = kafkaProducer;
        _logger = logger;
    }
    
    public async Task Handle(ShoppingCartTimedOutEvent notification, CancellationToken cancellationToken)
    {
        _kafkaProducer.Publish("shopping-cart", "shopping-cart-timed-out", notification);
        _logger.LogInformation($"Published shopping-cart-timed-out event {notification}");
    }
}