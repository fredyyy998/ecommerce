using Confluent.Kafka;
using Ecommerce.Common.Core;
using Ecommerce.Common.Kafka;
using Fulfillment.Core.DomainEvents;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Fulfillment.Infrastructure;

public class KafkaAccountConsumer : KafkaConsumer<string, string>
{
    private string _topic = "account";
    
    private readonly IMediator _mediator;

    private readonly ILogger<KafkaAccountConsumer> _logger;

    public KafkaAccountConsumer(IConfiguration configuration, IMediator mediator, ILogger<KafkaAccountConsumer> logger)
        : base(configuration["Kafka:BootstrapServers"], configuration["Kafka:GroupId"], "account")
    {
        _mediator = mediator;
        _logger = logger;
    }

    public override void HandleResult(ConsumeResult<string, string> consumeResult)
    {
        _logger.LogInformation("{Topic} Received event {Key} with {values}", _topic, consumeResult.Message.Key, consumeResult.Message.Value);
        try
        {
            IDomainEvent eventData = GetEventData(consumeResult.Message);
            _mediator.Publish(eventData);
        }
        catch (UnknownEventKeyException e)
        {
            _logger.LogInformation(e, "Unknown event key: {Key}", consumeResult.Message.Key);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while handling event {Key}", consumeResult.Message.Key);
        }
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
                throw new UnknownEventKeyException(message.Key);
        }
    }
}