using Ecommerce.Common.Kafka;
using Ecommerce.Common.Web;
using MediatR;
using ShoppingCart.Application.EventConsumer;
using ShoppingCart.Application.EventHandlers;
using ShoppingCart.Core.Events;
using ShoppingCart.Infrastructure.Kafka;

namespace ShoppingCart.Web.Configuration;

public class MessegeBusServiceInstaller : IServiceInstaller
{
    public void InstallService(IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(new KafkaProducer(configuration["Kafka:BootstrapServers"], configuration["Kafka:ClientId"]));

        services
            .AddScoped<INotificationHandler<CustomerAddedProductToBasketEvent>, CustomerAddedProductToBasketEventHandler>();
        services
            .AddScoped<INotificationHandler<CustomerChangedProductQuantityInCartEvent>,
                CustomerChangedProductQuantityInCartEventHandler>();
        services
            .AddScoped<INotificationHandler<CustomerOrderedShoppingCartEvent>, CustomerOrderedShoppingCartEventHandler>();
        services.AddScoped<INotificationHandler<ShoppingCartTimedOutEvent>, ShoppingCartTimedOutEventHandler>();
        services
            .AddScoped<INotificationHandler<ReservationCanceledDueToStockUpdateEvent>,
                ReservationCanceledDueToStockUpdateEventHandler>();
    
        services.AddHostedService<KafkaInventoryListener>();
        services.AddTransient<INotificationHandler<ProductAddedByAdminEvent>, ProductAddedByAdminEventConsumer>();
        services.AddTransient<INotificationHandler<ProductRemovedByAdminEvent>, ProductRemovedByAdminEventConsumer>();
        services.AddTransient<INotificationHandler<ProductStockUpdatedByAdminEvent>, ProductStockUpdateByAdminEventConsumer>();
        services.AddTransient<INotificationHandler<ProductUpdatedByAdminEvent>, ProductUpdatedByAdminEventConsumer>();
    }
}