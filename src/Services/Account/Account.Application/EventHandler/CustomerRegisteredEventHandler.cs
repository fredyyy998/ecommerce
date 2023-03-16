using Account.Core.Events;
using Account.Infrastructure.MessageBus;
using MediatR;

namespace Account.Application.EventHandler;

public class CustomerRegisteredEventHandler : INotificationHandler<CustomerRegisteredEvent>
{
    public readonly IMessageBus _messageBus;
    
    public CustomerRegisteredEventHandler(IMessageBus messageBus)
    {
        _messageBus = messageBus;
    }

    public async Task Handle(CustomerRegisteredEvent notification, CancellationToken cancellationToken)
    {
        _messageBus.Publish("customer-registration", "Customer Registered");
    }
}