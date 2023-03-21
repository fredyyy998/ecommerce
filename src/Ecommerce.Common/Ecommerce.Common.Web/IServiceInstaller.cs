using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ecommerce.Common.Web;

public interface IServiceInstaller
{
    void InstallService(IServiceCollection services, IConfiguration configuration);
}