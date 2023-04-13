
using System.Reflection;
using System.Text;
using Ecommerce.Common.Kafka;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Quartz;
using ShoppingCart.Application.EventConsumer;
using ShoppingCart.Application.EventHandlers;
using ShoppingCart.Application.Services;
using ShoppingCart.Application.Utils;
using ShoppingCart.Core.Events;
using ShoppingCart.Core.Product;
using ShoppingCart.Core.ShoppingCart;
using ShoppingCart.Infrastructure;
using ShoppingCart.Infrastructure.Kafka;
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

    builder.Services.AddSingleton<KafkaProducer>(new KafkaProducer(configuration["Kafka:BootstrapServers"], configuration["Kafka:ClientId"]));
    builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
    builder.Services
        .AddScoped<INotificationHandler<CustomerAddedProductToBasketEvent>, CustomerAddedProductToBasketEventHandler>();
    builder.Services
        .AddScoped<INotificationHandler<CustomerChangedProductQuantityInCartEvent>,
            CustomerChangedProductQuantityInCartEventHandler>();
    builder.Services
        .AddScoped<INotificationHandler<CustomerOrderedShoppingCartEvent>, CustomerOrderedShoppingCartEventHandler>();
    builder.Services.AddScoped<INotificationHandler<ShoppingCartTimedOutEvent>, ShoppingCartTimedOutEventHandler>();
    builder.Services
        .AddScoped<INotificationHandler<ReservationCanceledDueToStockUpdateEvent>,
            ReservationCanceledDueToStockUpdateEventHandler>();
    
    builder.Services.AddHostedService<KafkaInventoryListener>();
    builder.Services.AddTransient<INotificationHandler<ProductAddedByAdminEvent>, ProductAddedByAdminEventConsumer>();
    builder.Services.AddTransient<INotificationHandler<ProductRemovedByAdminEvent>, ProductRemovedByAdminEventConsumer>();
    builder.Services.AddTransient<INotificationHandler<ProductStockUpdatedByAdminEvent>, ProductStockUpdateByAdminEventConsumer>();
    builder.Services.AddTransient<INotificationHandler<ProductUpdatedByAdminEvent>, ProductUpdatedByAdminEventConsumer>();
}

var app = builder.Build();
{
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