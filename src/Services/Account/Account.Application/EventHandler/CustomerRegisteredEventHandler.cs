﻿using Account.Core.Events;
using Ecommerce.Common.Kafka;
using MediatR;

namespace Account.Application.EventHandler;

public class CustomerRegisteredEventHandler : INotificationHandler<CustomerRegisteredEvent>
{
    public readonly KafkaProducer _messageBus;
    
    public CustomerRegisteredEventHandler(KafkaProducer messageBus)
    {
        _messageBus = messageBus;
    }

    public async Task Handle(CustomerRegisteredEvent notification, CancellationToken cancellationToken)
    {
        _messageBus.Publish<CustomerRegisteredEvent>("customer", "customer-registration", notification);
    }
}