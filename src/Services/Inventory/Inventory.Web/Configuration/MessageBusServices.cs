using Ecommerce.Common.Kafka;
using Ecommerce.Common.Web;
using Inventory.Application.EventConsumer;
using Inventory.Application.EventHandlers;
using Inventory.Core.DomainEvents;
using Inventory.Infrastructure.Kafka;
using MediatR;

namespace Inventory.Web.Configuration;

public class MessageBusServices : IServiceInstaller
{
    public void InstallService(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(new KafkaProducer(configuration["Kafka:BootstrapServers"], configuration["Kafka:ClientId"]));
        services.AddScoped<INotificationHandler<ProductAddedByAdminEvent>, ProductAddedByAdminEventHandler>();
        services.AddScoped<INotificationHandler<ProductRemovedByAdmin>, ProductRemovedByAdminEventHandler>();
        services.AddScoped<INotificationHandler<ProductUpdatedByAdminEvent>, ProductUpdatedByAdminEventHandler>();
        services.AddScoped<INotificationHandler<ProductStockUpdated>, ProductStockUpdateEventHandler>();

        services.AddHostedService<ShoppingCartConsumer>();
        services
            .AddTransient<
                INotificationHandler<CustomerOrderedShoppingCartEvent>, CustomerOrderedShoppingCartEventConsumer>();

        services.AddHostedService<FulfillmentConsumer>();
        services
            .AddTransient<
                INotificationHandler<OrderCancelledEvent>, OrderCancelledEventConsumer>();
    }
}