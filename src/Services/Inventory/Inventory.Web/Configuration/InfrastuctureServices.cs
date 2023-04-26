using System.Reflection;
using Ecommerce.Common.Web;
using Inventory.Core.Product;
using Inventory.Infrastructure;
using Inventory.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Web.Configuration;

public class InfrastuctureServices : IServiceInstaller
{
    public void InstallService(IServiceCollection services, IConfiguration configuration)
    { 
        services.AddDbContext<DataContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly("Inventory.Web")));

        services.AddScoped<IProductRepository, ProductRepository>();
        
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
    }
}