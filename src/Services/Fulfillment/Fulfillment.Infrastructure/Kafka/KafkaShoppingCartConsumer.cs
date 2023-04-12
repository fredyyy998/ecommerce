using System.Text.Json;
using Confluent.Kafka;
using Ecommerce.Common.Core;
using Ecommerce.Common.Kafka;
using Fulfillment.Core.DomainEvents;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Fulfillment.Infrastructure;

public class KafkaShoppingCartConsumer : KafkaConsumer<string, string>
{
    private readonly IMediator _mediator;
    
    public KafkaShoppingCartConsumer(IConfiguration configuration, IMediator mediator)
        : base(configuration["Kafka:BootstrapServers"], configuration["Kafka:GroupId"], "shopping-cart")
    {
        _mediator = mediator;
    }
    
    public override void HandleResult(ConsumeResult<string, string> consumeResult)
    {
        IDomainEvent eventData = GetEventData(consumeResult.Message);
        
        Console.WriteLine(consumeResult.Message.Key);
        Console.WriteLine(consumeResult.Message.Value);
        
        _mediator.Publish(eventData);
    }
    
    private IDomainEvent GetEventData(Message<string, string> message)
    {
        switch (message.Key)
        {
            case "customer-ordered-shopping-cart":
                return JsonSerializer.Deserialize<CustomerOrderedShoppingCartEvent>(message.Value);

            default:
                throw new Exception("Unknown event type");
        }
    }
}