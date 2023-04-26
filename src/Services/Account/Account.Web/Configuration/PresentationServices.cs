using System.Reflection;
using Ecommerce.Common.Web;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

namespace Account.Web.Configuraiton;

public class PresentationServices : IServiceInstaller
{
    public void InstallService(IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddHttpContextAccessor();
        services.AddSwaggerGen();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo()
            {
                Title = "Account Service API", Version = "v1",
                Description = "The account service is a microservice of the Ecommerce application (https://github.com/fredyyy998/ecommerce). It is responsible for managing the user accounts of the application.",
            
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