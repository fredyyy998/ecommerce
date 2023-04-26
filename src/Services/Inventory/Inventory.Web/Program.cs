
using System.Reflection;
using System.Text;
using Ecommerce.Common.Kafka;
using Inventory.Application.EventConsumer;
using Inventory.Application.EventHandlers;
using Inventory.Application.Services;
using Inventory.Application.Utils;
using Inventory.Core.DomainEvents;
using Inventory.Core.Product;
using Inventory.Infrastructure;
using Inventory.Infrastructure.Kafka;
using Inventory.Infrastructure.Repository;
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
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddHttpContextAccessor();
    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "Inventory Service API", Version = "v1",
            Description = "The Inventory service is a microservice of the Ecommerce application [Github](https://github.com/fredyyy998/ecommerce). It is responsible for managing the products of the application and searching for available offerings."
        });
        
        options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme()
        {
            Description = "Standard authorization header using the Bearer scheme (\"bearer {token}\"",
            In = ParameterLocation.Header,
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey
        });

        options.OperationFilter<SecurityRequirementsOperationFilter>();
        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFile));
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

    builder.Services.AddHostedService<ShoppingCartConsumer>();
    builder.Services
        .AddTransient<
            INotificationHandler<CustomerOrderedShoppingCartEvent>, CustomerOrderedShoppingCartEventConsumer>();

    builder.Services.AddHostedService<FulfillmentConsumer>();
    builder.Services
        .AddTransient<
            INotificationHandler<OrderCancelledEvent>, OrderCancelledEventConsumer>();

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