using Ecommerce.Common.Kafka;
using Fulfillment.Core.DomainEvents;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Fulfillment.Application.EventHandler;

public class AdministratorShippedOrderEventHandler : INotificationHandler<AdministratorShippedOrderEvent>
{
    private readonly KafkaProducer _kafkaProducer;
    private readonly ILogger<AdministratorShippedOrderEventHandler> _logger;

    public AdministratorShippedOrderEventHandler(KafkaProducer kafkaProducer, ILogger<AdministratorShippedOrderEventHandler> logger)
    {
        _kafkaProducer = kafkaProducer;
        _logger = logger;
    }
    
    public Task Handle(AdministratorShippedOrderEvent notification, CancellationToken cancellationToken)
    {
        _kafkaProducer.Publish("fulfillment", "administrator-shipped-order", notification);
        _logger.LogInformation($"Published administrator shipped order {notification.OrderId}");
        return Task.CompletedTask;
    }
}