using System.Text.Json;
using Confluent.Kafka;

namespace Ecommerce.Common.Kafka;

public class KafkaProducer
{

    private readonly ProducerConfig _config;
    
    public KafkaProducer(string bootstrapServers, string clientId)
    {
        _config = new ProducerConfig()
        {    
            BootstrapServers = bootstrapServers,
            ClientId = clientId,
            EnableIdempotence = true,
            MessageTimeoutMs = 5000,
            CompressionType = CompressionType.Gzip,
            LingerMs = 500,
            BatchSize = 16384
        };
    }

    public DeliveryResult<string, string> Publish<T>(string topic, string key, T message)
    {
        var json = JsonSerializer.Serialize(message);
        
        using (var producer = 
               new ProducerBuilder<string, string>(_config).Build())
        {
            try
            {
                return producer.ProduceAsync(topic, new Message<string, string> { Key = key, Value = json })
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