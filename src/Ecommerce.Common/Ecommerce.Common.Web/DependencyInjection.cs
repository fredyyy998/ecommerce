using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ecommerce.Common.Web;

public static class DependencyInjection
{
    public static IServiceCollection InstallServices(
        this IServiceCollection services,
        IConfiguration configuration,
        params Assembly[] assemblies)
    {

        IEnumerable<IServiceInstaller> serviceInstallers = assemblies
            .SelectMany(assembly => assembly.DefinedTypes)
            .Where(IsAssignableType<IServiceInstaller>)
            .Select(Activator.CreateInstance)
            .Cast<IServiceInstaller>();

        foreach (var serviceInstaller in serviceInstallers)
        {
            serviceInstaller.InstallService(services, configuration);
        }

        return services;
        
        static bool IsAssignableType<T>(TypeInfo typeInfo) where T : notnull => typeof (T).IsAssignableFrom((Type) typeInfo) && !typeInfo.IsInterface && !typeInfo.IsAbstract;
    }
}