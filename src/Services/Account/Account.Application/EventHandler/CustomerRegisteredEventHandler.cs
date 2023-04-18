using Account.Core.Events;
using Ecommerce.Common.Kafka;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Account.Application.EventHandler;

public class CustomerRegisteredEventHandler : INotificationHandler<CustomerRegisteredEvent>
{
    private readonly KafkaProducer _messageBus;
    private readonly ILogger<CustomerRegisteredEventHandler> _logger;
    
    public CustomerRegisteredEventHandler(KafkaProducer messageBus, ILogger<CustomerRegisteredEventHandler> logger)
    {
        _messageBus = messageBus;
        _logger = logger;
    }

    public async Task Handle(CustomerRegisteredEvent notification, CancellationToken cancellationToken)
    {
        _messageBus.Publish<CustomerRegisteredEvent>("account", "customer-registered", notification);
        _logger.LogInformation($"Published Customer Registered Event for Customer Id: {notification.CustomerId}");
    }
}