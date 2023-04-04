using Confluent.Kafka;
using Ecommerce.Common.Core;
using Ecommerce.Common.Kafka;
using MediatR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ShoppingCart.Core.Events;

namespace ShoppingCart.Infrastructure.Kafka;

public class KafkaInventoryListener : KafkaConsumer<string, string>
{

    private readonly IMediator _mediator;
    
    public KafkaInventoryListener(IConfiguration configuration, IMediator mediator) 
        : base(configuration["Kafka:BootstrapServers"], configuration["Kafka:GroupId"], "inventory")
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
            case "product-added-by-admin":
                return JsonConvert.DeserializeObject<ProductAddedByAdminEvent>(message.Value);
            case "product-removed-by-admin":
                return JsonConvert.DeserializeObject<ProductRemovedByAdminEvent>(message.Value);
            case "product-stock-updated":
                return JsonConvert.DeserializeObject<ProductStockUpdatedByAdminEvent>(message.Value);
            case "product-updated-by-admin":
                return JsonConvert.DeserializeObject<ProductUpdatedByAdminEvent>(message.Value);
            default:
                throw new Exception("Unknown event type");
        }
    }
}