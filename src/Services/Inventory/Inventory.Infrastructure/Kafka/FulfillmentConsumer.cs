using Confluent.Kafka;
using Ecommerce.Common.Core;
using Ecommerce.Common.Kafka;
using Inventory.Core.DomainEvents;
using MediatR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Inventory.Infrastructure.Kafka;

public class FulfillmentConsumer : KafkaConsumer<string, string>
{
    public readonly IMediator _mediator;
    
    public FulfillmentConsumer(IConfiguration configuration, IMediator mediator) 
        : base(configuration["Kafka:BootstrapServers"], configuration["Kafka:GroupId"], "fulfillment")
    {
        _mediator = mediator;
    }
    
    public override void HandleResult(ConsumeResult<string, string> consumeResult)
    {
        Console.WriteLine(consumeResult.Message.Key);
        Console.WriteLine(consumeResult.Message.Value);
        try
        {
            IDomainEvent eventData = GetEventData(consumeResult.Message);
            _mediator.Publish(eventData);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

    }
    
    private IDomainEvent GetEventData(Message<string, string> message)
    {
        switch (message.Key)
        {
            case "order-cancelled":
                return JsonConvert.DeserializeObject<OrderCancelledEvent>(message.Value);
            default:
                throw new Exception("Unknown event type");
        }
    }
}