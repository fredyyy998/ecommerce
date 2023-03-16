using Account.Core.Events;
using Account.Infrastructure.MessageBus;
using MediatR;

namespace Account.Application.EventHandler;

public class CustomerEditedEventHandler : INotificationHandler<CustomerEditedEvent>
{
    public readonly IMessageBus _messageBus;

    public CustomerEditedEventHandler(IMessageBus messageBus)
    {
        _messageBus = messageBus;
    }

    public async Task Handle(CustomerEditedEvent notification, CancellationToken cancellationToken)
    {
        _messageBus.Publish("customer-edited", "Customer edited");
    }
}