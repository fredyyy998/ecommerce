using System.Text;
using Account.Application;
using Ecommerce.Common.Web;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Account.Web.Configuration;

public class AuthorizationAndAuthenticationInstaller : IServiceInstaller
{
    public void InstallService(IServiceCollection services, IConfiguration configuration)
    {
        var jwtInformation = new JwtInformation(configuration["JWT:Secret"], configuration["JWT:ValidIssuer"], configuration["JWT:ValidAudience"]);
        services.AddSingleton<JwtInformation>(jwtInformation);
    
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey =
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("JWT:Secret").Value)),
                ValidateIssuer = false,
                ValidateAudience = false
            };
        });
    }
}