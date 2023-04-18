using Ecommerce.Common.Kafka;
using Fulfillment.Core.DomainEvents;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fulfillment.Application.EventHandler;

public class OrderCancelledEventHandler : INotificationHandler<OrderCancelledEvent>
{
    private readonly KafkaProducer _kafkaProducer;
    private readonly ILogger<OrderCancelledEventHandler> _logger;
    
    public OrderCancelledEventHandler(KafkaProducer kafkaProducer, ILogger<OrderCancelledEventHandler> logger)
    {
        _kafkaProducer = kafkaProducer;
        _logger = logger;
    }
    
    public Task Handle(OrderCancelledEvent notification, CancellationToken cancellationToken)
    {
        _kafkaProducer.Publish("fulfillment", "order-cancelled", notification);
        _logger.LogInformation($"Published order cancelled {notification.OrderId}");
        return Task.CompletedTask;
    }
}