using Confluent.Kafka;
using Ecommerce.Common.Core;
using Ecommerce.Common.Kafka;
using Fulfillment.Core.DomainEvents;
using MediatR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

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
        try
        {
            IDomainEvent eventData = GetEventData(consumeResult.Message);
        
            Console.WriteLine(consumeResult.Message.Key);
            Console.WriteLine(consumeResult.Message.Value);
        
            _mediator.Publish(eventData);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
    
    private IDomainEvent GetEventData(Message<string, string> message)
    {
        switch (message.Key)
        {
            case "customer-ordered-shopping-cart":
                return JsonConvert.DeserializeObject<CustomerOrderedShoppingCartEvent>(message.Value);

            default:
                throw new Exception("Unknown event type");
        }
    }
}