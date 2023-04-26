using System.Reflection;
using Ecommerce.Common.Web;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

namespace Inventory.Web.Configuration;

public class PresentationServices : IServiceInstaller
{
    public void InstallService(IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddHttpContextAccessor();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo()
            {
                Title = "Inventory Service API", Version = "v1",
                Description = "The Inventory service is a microservice of the Ecommerce application [Github](https://github.com/fredyyy998/ecommerce). It is responsible for managing the products of the application and searching for available offerings."
            });
        
            options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme()
            {
                Description = "Standard authorization header using the Bearer scheme (\"bearer {token}\"",
                In = ParameterLocation.Header,
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey
            });

            options.OperationFilter<SecurityRequirementsOperationFilter>();
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFile));
        });
    }
}