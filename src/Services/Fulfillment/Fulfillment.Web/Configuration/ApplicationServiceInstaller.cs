using Ecommerce.Common.Web;
using Fulfillment.Application.Services;
using Fulfillment.Application.Services.Revenue;
using Fulfillment.Application.Utlis;

namespace Fulfillment.Web.Configuration;

public class ApplicationServiceInstaller : IServiceInstaller
{
    public void InstallService(IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(typeof(MappingProfile));
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IRevenueService, RevenueReportService>();
    }
}