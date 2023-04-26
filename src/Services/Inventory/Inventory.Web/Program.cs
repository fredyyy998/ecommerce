using System.Reflection;
using Ecommerce.Common.Web;
using Inventory.Infrastructure;

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


// seed work
using (var scope = builder.Services.BuildServiceProvider().CreateScope())
{
    var dataContext = scope.ServiceProvider.GetService<DataContext>();
    await DataSeeder.SeedProducts(dataContext);
}



var app = builder.Build();
{
    // allow cors
    app.UseCors(LocalDevelopmentOrigins);
    
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