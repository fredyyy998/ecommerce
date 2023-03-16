using System.Text.Json;
using Confluent.Kafka;

namespace Account.Infrastructure.MessageBus;

public class KafkaProducer : IMessageBus
{
    
    private readonly ProducerConfig config = new ProducerConfig 
        {    
            BootstrapServers = "localhost:9092",
            ClientId = "account-service",
            EnableIdempotence = true,
            MessageTimeoutMs = 5000,
            CompressionType = CompressionType.Gzip,
            LingerMs = 500,
            BatchSize = 16384
        };
    
    public Object Publish<T>(string topic, T message)
    {
        var json = JsonSerializer.Serialize(message);
        
        using (var producer = 
               new ProducerBuilder<string, string>(config).Build())
        {
            try
            {
                return producer.ProduceAsync(topic, new Message<string, string> { Key = "Test-Key", Value = json })
                    .GetAwaiter()
                    .GetResult();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Oops, something went wrong: {e}");
            }
        }
        return null;
    }
}