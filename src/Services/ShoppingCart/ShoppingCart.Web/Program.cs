
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Quartz;
using ShoppingCart.Application.Services;
using ShoppingCart.Application.Utils;
using ShoppingCart.Core.Product;
using ShoppingCart.Core.ShoppingCart;
using ShoppingCart.Infrastructure;
using ShoppingCart.Infrastructure.Repositories;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);
{
    ConfigurationManager configuration = builder.Configuration;
    
    // Add services to the container.
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddHttpContextAccessor();
    builder.Services.AddSwaggerGen(options =>
    {
        options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme()
        {
            Description = "Standard authorization header using the Bearer scheme (\"bearer {token}\"",
            In = ParameterLocation.Header,
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey
        });

        options.OperationFilter<SecurityRequirementsOperationFilter>();
    });
    
    builder.Services.AddDbContext<DataContext>(options =>
        options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
            b => b.MigrationsAssembly("ShoppingCart.Web")));


    builder.Services.AddScoped<IProductRepository, ProductRepository>();
    builder.Services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();
    
    builder.Services.AddAutoMapper(typeof(MappingProfile));    
    builder.Services.AddScoped<IShoppingCartService, ShoppingCartService>();

    builder.Services.AddQuartz(q =>
    {
        q.UseMicrosoftDependencyInjectionScopedJobFactory();
        var jobKey = new JobKey("TimeOutShoppingCartsJob");
        q.AddJob<TimeOutShoppingCartsJob>(opts => opts.WithIdentity(jobKey));
        q.AddTrigger(opts => opts
            .ForJob(jobKey)
            .WithIdentity("TimeOutShoppingCartsJobTrigger")
            .WithCronSchedule("0 */30 * ? * *"));
    });
    builder.Services.AddQuartzHostedService(opt => opt.WaitForJobsToComplete = true);
}

var app = builder.Build();
{
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseCors("corsapp");
    app.UseExceptionHandler("/error");
    app.UseHttpsRedirection();
    app.MapControllers();

    app.UseAuthentication();
    app.UseAuthorization();
    
    app.Run();
}

public partial class Program { }