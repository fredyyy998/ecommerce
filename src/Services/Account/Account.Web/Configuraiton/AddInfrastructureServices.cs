using System.Reflection;
using Account.Core.Administrator;
using Account.Core.User;
using Account.Infrastructure;
using Account.Infrastructure.Repository;
using Ecommerce.Common.Web;
using Microsoft.EntityFrameworkCore;

namespace Account.Web.Configuraiton;

public class AddInfrastructureServices : IServiceInstaller
{
    public void InstallService(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<DataContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly("Account.Web")));

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<IAdministratorRepository, AdministratorRepository>();
    }
}