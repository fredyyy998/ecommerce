using System.Reflection;

namespace Account.Web.Configuration;

public static class DependencyInjection
{
    public static IServiceCollection InstallServices(
        this IServiceCollection services,
        IConfiguration configuration,
        params Assembly[] assemblies)
    {
        IEnumerable<IServiceInstaller> serviceInstallers = assemblies
            .SelectMany(x => x.DefinedTypes)
            .Where(IsAssignableType<IServiceInstaller>)
            .Select(Activator.CreateInstance)
            .Cast<IServiceInstaller>();
        
        foreach (var serviceInstaller in serviceInstallers)
        {
            serviceInstaller.InstallService(services, configuration);
        }
        return services;
        
        static bool IsAssignableType<T>(TypeInfo typeInfo) => 
            typeof(T).IsAssignableFrom(typeInfo) &&
            !typeInfo.IsInterface &&
            !typeInfo.IsAbstract;
    }
}