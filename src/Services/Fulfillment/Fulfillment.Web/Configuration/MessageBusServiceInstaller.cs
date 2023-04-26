using Ecommerce.Common.Kafka;
using Ecommerce.Common.Web;
using Fulfillment.Application.EventConsumer;
using Fulfillment.Application.EventHandler;
using Fulfillment.Core.DomainEvents;
using Fulfillment.Infrastructure;
using MediatR;

namespace Fulfillment.Web.Configuration;

public class MessageBusServiceInstaller : IServiceInstaller
{
    public void InstallService(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(new KafkaProducer(configuration["Kafka:BootstrapServers"], configuration["Kafka:ClientId"]));
    
        services
            .AddScoped<INotificationHandler<AdministratorShippedOrderEvent>, AdministratorShippedOrderEventHandler>();
        services.AddScoped<INotificationHandler<OrderCancelledEvent>, OrderCancelledEventHandler>();

        services.AddHostedService<KafkaAccountConsumer>();
        services.AddHostedService<KafkaShoppingCartConsumer>();
        services.AddTransient<INotificationHandler<BuyerPaidOrderEvent>, BuyerPaidOrderEventConsumer>();
        services
            .AddTransient<INotificationHandler<CustomerOrderedShoppingCartEvent>, CustomerOrderedShoppingCartEventConsumer>();
        services.AddTransient<INotificationHandler<CustomerEditedEvent>, CustomerEditedEventConsumer>();
        services.AddTransient<INotificationHandler<CustomerRegisteredEvent>, CustomerRegisteredEventConsumer>();
        services
            .AddTransient<INotificationHandler<LogisticProviderDeliveredOrderEvent>,
                LogisticProviderDeliveredOrderEventConsumer>();
    }
}