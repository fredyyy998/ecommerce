using System.Reflection;
using Ecommerce.Common.Web;

namespace Account.Web.Configuration;

public static class DependencyInjection
{
    public static IServiceCollection InstallServicesTest(
        this IServiceCollection services,
        IConfiguration configuration,
        params Assembly[] assemblies)
    {
        foreach (IServiceInstaller serviceInstaller in ((IEnumerable<Assembly>) assemblies).SelectMany<Assembly, TypeInfo>((Func<Assembly, IEnumerable<TypeInfo>>) (x => x.DefinedTypes)).Where<TypeInfo>(new Func<TypeInfo, bool>(IsAssignableType<IServiceInstaller>)).Select<TypeInfo, object>(new Func<TypeInfo, object>(Activator.CreateInstance)).Cast<IServiceInstaller>())
            serviceInstaller.InstallService(services, configuration);
        return services;

        static bool IsAssignableType<T>(TypeInfo typeInfo) where T : notnull => typeof (T).IsAssignableFrom((Type) typeInfo) && !typeInfo.IsInterface && !typeInfo.IsAbstract;
    }
}
