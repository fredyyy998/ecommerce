using Account.Application;
using Account.Application.Dtos;
using Account.Application.Profile;
using Ecommerce.Common.Web;
using FluentValidation;

namespace Account.Web.Configuration;

public class ApplicationServiceInstaller : IServiceInstaller
{
    public void InstallService(IServiceCollection services, IConfiguration configuration)
    {
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IProfileService, ProfileService>();
            
            services.AddScoped<IValidator<CustomerCreateDto>, CustomerCreateDtoValidator>();
            services.AddAutoMapper(typeof(MappingProfile));
    }
}