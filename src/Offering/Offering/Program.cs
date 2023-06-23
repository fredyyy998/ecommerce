

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
    builder.Services.AddScoped<ILocalizationRepository, LocalizationRepository>();

    builder.Services.AddSwaggerGen();
}

var app = builder.Build();
{
    using (var scope = app.Services.CreateScope())
    {
        var dataContext = scope.ServiceProvider.GetService<DataContext>();
        await DataSeeder.SeedData(dataContext);
    }
    app.UseExceptionHandler("/error");
    app.UseHttpsRedirection();
    app.MapControllers();
    
    app.UseSwagger();
    app.UseSwaggerUI();
    
    app.Run();
}