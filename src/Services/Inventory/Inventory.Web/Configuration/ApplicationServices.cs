using Ecommerce.Common.Web;
using Inventory.Application.Services;
using Inventory.Application.Utils;

namespace Inventory.Web.Configuration;

public class ApplicationServices : IServiceInstaller
{
    public void InstallService(IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(typeof(MappingProfile));
        services.AddScoped<IProductService, ProductService>();
    }
}