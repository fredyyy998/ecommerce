using System.Text.Json;
using Confluent.Kafka;

namespace Ecommerce.Common.Kafka;

public class KafkaConsumer
{
    private readonly ConsumerConfig _config;
    
    public KafkaConsumer(string bootstrapServers, string clientId)
    {
        _config = new ConsumerConfig()
        {    
            BootstrapServers = bootstrapServers,
            ClientId = clientId,
            GroupId = "my-group-id",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };
    }
    
    public void Subscribe<T>(string topic, Action<T> handler)
    {
        using (var consumer = new ConsumerBuilder<string, string>(_config).Build())
        {
            consumer.Subscribe(topic);
            
            while (true)
            {
                var consumeResult = consumer.Consume();
                var message = JsonSerializer.Deserialize<T>(consumeResult.Message.Value);
                handler(message);
            }
        }
    }
}