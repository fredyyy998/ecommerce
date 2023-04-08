using Account.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Account.IntegrationTest;

public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((context, config) =>
        {
            context.HostingEnvironment.ContentRootPath = Path.Combine(Directory.GetCurrentDirectory());

            config.AddJsonFile("appsettings.IntegrationTest.json");
        });
        
        builder.ConfigureServices(services =>
        {
            var dbContextDescriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                     typeof(DbContextOptions<DataContext>));

            services.Remove(dbContextDescriptor);

            var configuration = services.BuildServiceProvider().GetService<IConfiguration>();

            services.AddDbContext<DataContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly("Account.Web")));
        });

        builder.UseEnvironment("Development");

    }
}