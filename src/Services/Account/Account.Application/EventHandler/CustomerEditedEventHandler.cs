using Account.Core.Events;
using Ecommerce.Common.Kafka;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Account.Application.EventHandler;

public class CustomerEditedEventHandler : INotificationHandler<CustomerEditedEvent>
{
    private readonly KafkaProducer _messageBus;
    private readonly ILogger<CustomerEditedEventHandler> _logger;

    public CustomerEditedEventHandler(KafkaProducer messageBus, ILogger<CustomerEditedEventHandler> logger)
    {
        _messageBus = messageBus;
        _logger = logger;
    }

    public async Task Handle(CustomerEditedEvent notification, CancellationToken cancellationToken)
    {
        _messageBus.Publish<CustomerEditedEvent>("account", "customer-edited", notification);
        _logger.LogInformation($"Published Customer Edited Event for Customer Id: {notification.CustomerId}");
    }
}