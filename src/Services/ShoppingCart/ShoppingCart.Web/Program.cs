using System.Reflection;
using Ecommerce.Common.Web;

var  LocalDevelopmentOrigins = "_localDevelopmentOrigins";

var builder = WebApplication.CreateBuilder(args);
{
    ConfigurationManager configuration = builder.Configuration;

    builder.Services.InstallServices(configuration, Assembly.GetExecutingAssembly());
    
    
    builder.Services.AddCors(options =>
    {
        options.AddPolicy(name: LocalDevelopmentOrigins,
            policy  =>
            {
                policy.WithOrigins("http://localhost:4200")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
    });
}

var app = builder.Build();
{
    app.UseCors(LocalDevelopmentOrigins);
    
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseCors("corsapp");
    app.UseExceptionHandler("/error");
    app.UseHttpsRedirection();
    app.MapControllers();

    app.UseAuthentication();
    app.UseAuthorization();
    
    app.Run();
}

public partial class Program { }