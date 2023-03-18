namespace Account.Web.Configuration;

public interface IServiceInstaller
{
    void InstallService(IServiceCollection services, IConfiguration configuration);
}