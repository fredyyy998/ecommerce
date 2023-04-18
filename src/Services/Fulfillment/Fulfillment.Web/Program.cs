
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using Ecommerce.Common.Kafka;
using Fulfillment.Application.EventConsumer;
using Fulfillment.Application.EventHandler;
using Fulfillment.Application.Services;
using Fulfillment.Application.Utlis;
using Fulfillment.Core.Buyer;
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


var  LocalDevelopmentOrigins = "_localDevelopmentOrigins";

var builder = WebApplication.CreateBuilder(args);
{
    ConfigurationManager configuration = builder.Configuration;

    // Add services to the container.
    builder.Services.AddControllers().AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
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
    builder.Services.AddScoped<IBuyerRepository, BuyerRepository>();

    builder.Services.AddAutoMapper(typeof(MappingProfile));
    builder.Services.AddScoped<IOrderService, OrderService>();
    
    builder.Services.AddSingleton(new KafkaProducer(configuration["Kafka:BootstrapServers"], configuration["Kafka:ClientId"]));
    builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
    builder.Services
        .AddScoped<INotificationHandler<AdministratorShippedOrderEvent>, AdministratorShippedOrderEventHandler>();
    builder.Services.AddScoped<INotificationHandler<OrderCancelledEvent>, OrderCancelledEventHandler>();

    builder.Services.AddHostedService<KafkaAccountConsumer>();
    builder.Services.AddHostedService<KafkaShoppingCartConsumer>();
    builder.Services.AddTransient<INotificationHandler<BuyerPaidOrderEvent>, BuyerPaidOrderEventConsumer>();
    builder.Services
        .AddTransient<INotificationHandler<CustomerOrderedShoppingCartEvent>, CustomerOrderedShoppingCartEventConsumer>();
    builder.Services.AddTransient<INotificationHandler<CustomerEditedEvent>, CustomerEditedEventConsumer>();
    builder.Services.AddTransient<INotificationHandler<CustomerRegisteredEvent>, CustomerRegisteredEventConsumer>();
    builder.Services
        .AddTransient<INotificationHandler<LogisticProviderDeliveredOrderEvent>,
            LogisticProviderDeliveredOrderEventConsumer>();
    
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