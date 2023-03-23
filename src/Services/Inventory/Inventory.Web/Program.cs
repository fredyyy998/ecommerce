
using Inventory.Application.Services;
using Inventory.Application.Utils;
using Inventory.Core.Product;
using Inventory.Infrastructure;
using Inventory.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
{
    ConfigurationManager configuration = builder.Configuration;
    // Add services to the container.
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddHttpContextAccessor();
    builder.Services.AddSwaggerGen();
    
    builder.Services.AddDbContext<DataContext>(options =>
        options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
            b => b.MigrationsAssembly("Inventory.Web")));

    builder.Services.AddScoped<IProductRepository, ProductRepository>();
    
    builder.Services.AddAutoMapper(typeof(MappingProfile));
    builder.Services.AddScoped<IProductService, ProductService>();
    
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