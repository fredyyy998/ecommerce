using Confluent.Kafka;
using Ecommerce.Common.Core;
using MediatR;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using ShoppingCart.Core.Events;

namespace ShoppingCart.Infrastructure.Kafka;

public class KafkaListener : BackgroundService
{

    private readonly IConsumer<string, string> _consumer;

    private readonly IMediator _mediator;
    
    public KafkaListener(IMediator mediator)
    {
        _mediator = mediator;
        
        var config = new ConsumerConfig
        {
            BootstrapServers = "localhost:9092",
            GroupId = "my-group-id",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        var builder = new ConsumerBuilder<string, string>(config)
            .Build();

        builder.Subscribe("inventory");

        _consumer = builder;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var consumeResult = _consumer.Consume(stoppingToken);
            
            
            // Handle the message
            if (consumeResult != null)
            {
                IDomainEvent eventData = GetEventData(consumeResult.Message);
                
                // Check if the message is relevant and trigger the appropriate side effects
                Console.WriteLine("Received a hello message from Kafka!");
                Console.WriteLine(consumeResult.Message.Key);
                Console.WriteLine(consumeResult.Message.Value);

                _mediator.Publish(eventData);
            }
        }
    }

    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        _consumer.Close();
        await base.StopAsync(stoppingToken);
    }
    
    private IDomainEvent GetEventData(Message<string, string> message)
    {
        switch (message.Key)
        {
            case "product-added-by-admin":
                return JsonConvert.DeserializeObject<ProductAddedByAdminEvent>(message.Value);
            case "product-removed-by-admin":
                return JsonConvert.DeserializeObject<ProductRemovedByAdminEvent>(message.Value);
            default:
                throw new Exception("Unknown event type");
        }
    }
}