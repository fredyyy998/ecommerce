using Confluent.Kafka;
using Ecommerce.Common.Core;
using Ecommerce.Common.Kafka;
using Fulfillment.Core.DomainEvents;
using MediatR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Fulfillment.Infrastructure;

public class KafkaAccountConsumer : KafkaConsumer<string, string>
{
    private readonly IMediator _mediator;

    public KafkaAccountConsumer(IConfiguration configuration, IMediator mediator)
        : base(configuration["Kafka:BootstrapServers"], configuration["Kafka:GroupId"], "account")
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
            case "customer-registered":
                return JsonConvert.DeserializeObject<CustomerRegisteredEvent>(message.Value);
            case "customer-edited":
                return JsonConvert.DeserializeObject<CustomerEditedEvent>(message.Value);
            default:
                throw new Exception("Unknown event type");
        }
    }
}