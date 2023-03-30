
using System.Reflection;
using System.Text;
using Ecommerce.Common.Kafka;
using Inventory.Application.EventHandlers;
using Inventory.Application.Services;
using Inventory.Application.Utils;
using Inventory.Core.DomainEvents;
using Inventory.Core.Product;
using Inventory.Infrastructure;
using Inventory.Infrastructure.Repository;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
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
            b => b.MigrationsAssembly("Inventory.Web")));

    builder.Services.AddScoped<IProductRepository, ProductRepository>();
    
    builder.Services.AddAutoMapper(typeof(MappingProfile));
    builder.Services.AddScoped<IProductService, ProductService>();
    
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("JWT:Secret").Value)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

    builder.Services.AddSingleton<KafkaProducer>(new KafkaProducer(configuration["Kafka:BootstrapServers"], configuration["Kafka:ClientId"]));
    builder.Services.AddScoped<INotificationHandler<ProductAddedByAdminEvent>, ProductAddedByAdminEventHandler>();
    builder.Services.AddScoped<INotificationHandler<ProductRemovedByAdmin>, ProductRemovedByAdminEventHandler>();
    builder.Services.AddScoped<INotificationHandler<ProductUpdatedByAdminEvent>, ProductUpdatedByAdminEventHandler>();
    builder.Services.AddScoped<INotificationHandler<ProductStockUpdated>, ProductStockUpdateEventHandler>();
    
    builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

    // builder.Services.InstallServices(configuration, typeof(IServiceInstaller).Assembly);
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