using System.Reflection;
using Account.Application.EventHandler;
using Account.Core.Events;
using Account.Core.User;
using Account.Infrastructure;
using Account.Infrastructure.MessageBus;
using Account.Infrastructure.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Account.Web.Configuration;

public class AddInfrastructureServices : IServiceInstaller
{
    public void InstallService(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<DataContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly("Account.Web")));
    
    
    
        services.AddScoped<ICustomerRepository, CustomerRepository>();

        services.AddScoped<IMessageBus, KafkaProducer>();
        services.AddScoped<INotificationHandler<CustomerRegisteredEvent>, CustomerRegisteredEventHandler>();
        services.AddScoped<INotificationHandler<CustomerEditedEvent>, CustomerEditedEventHandler>();
    
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
    }
}