using System.Reflection;
using Ecommerce.Common.Web;

namespace Account.Web.Configuration;

public static class DependencyInjectionImplementation
{
    public static IServiceCollection InstallServicesTest(
        this IServiceCollection services,
        IConfiguration configuration,
        params Assembly[] assemblies)
    {
        return DependencyInjection.InstallServices(services, configuration, assemblies);
    }
}
