using Account.Core.Events;
using Ecommerce.Common.Kafka;
using MediatR;

namespace Account.Application.EventHandler;

public class CustomerEditedEventHandler : INotificationHandler<CustomerEditedEvent>
{
    public readonly KafkaProducer _messageBus;

    public CustomerEditedEventHandler(KafkaProducer messageBus)
    {
        _messageBus = messageBus;
    }

    public async Task Handle(CustomerEditedEvent notification, CancellationToken cancellationToken)
    {
        _messageBus.Publish<CustomerEditedEvent>("account", "customer-edited", notification);
    }
}