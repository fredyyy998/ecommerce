using System.Reflection;
using Ecommerce.Common.Web;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Core.Product;
using ShoppingCart.Core.ShoppingCart;
using ShoppingCart.Infrastructure;
using ShoppingCart.Infrastructure.Repositories;

namespace ShoppingCart.Web.Configuration;

public class InfrastuctureServiceInstaller : IServiceInstaller
{
    public void InstallService(IServiceCollection services, IConfiguration configuration)
    {
            
        services.AddDbContext<DataContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly("ShoppingCart.Web")));


        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();
        
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
    }
}