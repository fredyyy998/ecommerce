using System.Reflection;
using System.Text;
using Account.Application;
using Account.Application.Dtos;
using Account.Application.EventHandler;
using Account.Application.Profile;
using Account.Core.Events;
using Account.Core.User;
using Account.Infrastructure;
using Account.Infrastructure.MessageBus;
using Account.Infrastructure.Repository;
using FluentValidation;
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
    builder.Services.AddSwaggerGen();
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

    builder.Services.AddScoped<IValidator<CustomerCreateDto>, CustomerCreateDtoValidator>();
    builder.Services.AddAutoMapper(typeof(MappingProfile));
    builder.Services.AddDbContext<DataContext>(options =>
        options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
            b => b.MigrationsAssembly("Account.Web")));
    

    var jwtInformation = new JwtInformation(configuration["JWT:Secret"], configuration["JWT:ValidIssuer"], configuration["JWT:ValidAudience"]);
    builder.Services.AddSingleton<JwtInformation>(jwtInformation);
    
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
    
    builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

    builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
    builder.Services.AddScoped<IProfileService, ProfileService>();
    
    builder.Services.AddScoped<INotificationHandler<CustomerRegisteredEvent>, CustomerRegisteredEventHandler>();
    builder.Services.AddScoped<INotificationHandler<CustomerEditedEvent>, CustomerEditedEventHandler>();
    
    builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
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