using Confluent.Kafka;
using Microsoft.Extensions.Hosting;

namespace Ecommerce.Common.Kafka;

public abstract class KafkaConsumer<TKey, TMessage> : BackgroundService
{
    protected readonly IConsumer<TKey, TMessage> _consumer;

    public KafkaConsumer(string bootstrapServer, string groupId, string topic)
    {
        var config = new ConsumerConfig
        {
            BootstrapServers = bootstrapServer,
            GroupId = groupId,
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        var builder = new ConsumerBuilder<TKey, TMessage>(config)
            .Build();

        builder.Subscribe(topic);

        _consumer = builder;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Run(() =>
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var consumeResult = _consumer.Consume(stoppingToken);
                if (consumeResult != null)
                    HandleResult(consumeResult);
                consumeResult = null;
            }
        });
    }

    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        _consumer.Close();
        await base.StopAsync(stoppingToken);
    }
    
    public abstract void HandleResult(ConsumeResult<TKey, TMessage> consumeResult);
}