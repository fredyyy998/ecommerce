﻿

using Account.Application;
using Account.Application.Dtos;
using Account.Application.Profile;
using Ecommerce.Common.Web;
using FluentValidation;

namespace Account.Web.Configuration;

public class ApplicationServices : IServiceInstaller
{
    public void InstallService(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IValidator<CustomerCreateDto>, CustomerCreateDtoValidator>();
        services.AddAutoMapper(typeof(MappingProfile));
        
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IProfileService, ProfileService>();
    }
}