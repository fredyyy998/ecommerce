
using System.Reflection;
using System.Text;
using Ecommerce.Common.Kafka;
using Fulfillment.Application.EventConsumer;
using Fulfillment.Application.EventHandler;
using Fulfillment.Application.Services;
using Fulfillment.Application.Utlis;
using Fulfillment.Core.DomainEvents;
using Fulfillment.Core.Order;
using Fulfillment.Infrastructure;
using Fulfillment.Infrastructure.Repositories;
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
    
    builder.Services.AddDbContext<DataContext>(options =>
        options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
            b => b.MigrationsAssembly("Fulfillment.Web")));

    builder.Services.AddScoped<IOrderRepository, OrderRepository>();

    builder.Services.AddAutoMapper(typeof(MappingProfile));
    builder.Services.AddScoped<IOrderService, OrderService>();
    
    builder.Services.AddSingleton(new KafkaProducer(configuration["Kafka:BootstrapServers"], configuration["Kafka:ClientId"]));
    builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
    builder.Services
        .AddScoped<INotificationHandler<AdministratorShippedOrderEvent>, AdministratorShippedOrderEventHandler>();
    builder.Services
        .AddScoped<INotificationHandler<BuyerSubmittedOrderEvent>, BuyerSubmittedOrderEventHandler>();

    builder.Services.AddHostedService<KafkaAccountConsumer>();
    builder.Services.AddScoped<INotificationHandler<BuyerPaidOrderEvent>, BuyerPaidOrderEventConsumer>();
    builder.Services
        .AddScoped<INotificationHandler<CustomerOrderedShoppingCartEvent>, CustomerOrderedShoppingCartEventConsumer>();
    builder.Services.AddScoped<INotificationHandler<CustomerEditedEvent>, CustomerEditedEventConsumer>();
}

var app = builder.Build();
{
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