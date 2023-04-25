using System.Reflection;
using Ecommerce.Common.Web;

var  MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);
{
    ConfigurationManager configuration = builder.Configuration;

    builder.Services.AddCors(options =>
    {
        options.AddPolicy(name: MyAllowSpecificOrigins,
            policy  =>
            {
                policy.WithOrigins("http://localhost:4200")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
    });

    builder.Services.InstallServices(builder.Configuration, Assembly.GetExecutingAssembly());
}

var app = builder.Build();
{
    // allow cors
    app.UseCors(MyAllowSpecificOrigins);

    // Configure the HTTP request pipeline.
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