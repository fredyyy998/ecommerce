using Account.Application.EventHandler;
using Account.Core.Events;
using Ecommerce.Common.Kafka;
using Ecommerce.Common.Web;
using MediatR;

namespace Account.Web.Configuration;

public class MessageBusServices : IServiceInstaller
{
    public void InstallService(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(new KafkaProducer(configuration["Kafka:BootstrapServers"], configuration["Kafka:ClientId"]));
        services.AddScoped<INotificationHandler<CustomerRegisteredEvent>, CustomerRegisteredEventHandler>();
        services.AddScoped<INotificationHandler<CustomerEditedEvent>, CustomerEditedEventHandler>();
    }
}