

using Microsoft.EntityFrameworkCore;
using Offering.Repositories;

var builder = WebApplication.CreateBuilder(args);
{
    ConfigurationManager configuration = builder.Configuration;
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddHttpContextAccessor();
    
    // add data context with mysql
    builder.Services.AddDbContext<DataContext>(options =>
        options.UseMySQL(configuration.GetConnectionString("DefaultConnection")));

    builder.Services.AddScoped<IOfferRepository, OfferRepository>();
    builder.Services.AddScoped<IProductRepository, ProductRepository>();
}

var app = builder.Build();
{
    app.UseExceptionHandler("/error");
    app.UseHttpsRedirection();
    app.MapControllers();
    
    app.Run();
}