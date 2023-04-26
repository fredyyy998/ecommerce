using Ecommerce.Common.Web;
using ShoppingCart.Application.Services;
using ShoppingCart.Application.Utils;

namespace ShoppingCart.Web.Configuration;

public class ApplicationServiceInstaller : IServiceInstaller
{
    public void InstallService(IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(typeof(MappingProfile));    
        services.AddScoped<IShoppingCartService, ShoppingCartService>();
    }
}