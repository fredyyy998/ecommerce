using Account.Application;
using Account.Application.Dtos;
using Account.Application.Profile;
using Account.Core.User;
using Account.Infrastructure;
using Account.Infrastructure.Repository;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
{
    // Add services to the container.
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddHttpContextAccessor();
    builder.Services.AddSwaggerGen();

    builder.Services.AddScoped<IValidator<CustomerCreateDto>, CustomerCreateDtoValidator>();
    builder.Services.AddAutoMapper(typeof(MappingProfile));
    builder.Services.AddDbContext<DataContext>();

    // TODO exchange with real values
    var jwtInformation = new JwtInformation("0Ukke8V63dDaWqgX0Ukke8V63dDaWqgX", "testIssuer", "testAudience");
    builder.Services.AddSingleton<JwtInformation>(jwtInformation);
    
    builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

    builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
    builder.Services.AddScoped<IProfileService, ProfileService>();
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
    app.Run();
}