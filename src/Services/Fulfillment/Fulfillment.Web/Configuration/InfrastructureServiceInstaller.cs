using System.Reflection;
using Ecommerce.Common.Web;
using Fulfillment.Core.Buyer;
using Fulfillment.Core.Order;
using Fulfillment.Infrastructure;
using Fulfillment.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Fulfillment.Web.Configuration;

public class InfrastructureServiceInstaller : IServiceInstaller
{
    public void InstallService(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<DataContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly("Fulfillment.Web")));

        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IBuyerRepository, BuyerRepository>();

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
    }
}