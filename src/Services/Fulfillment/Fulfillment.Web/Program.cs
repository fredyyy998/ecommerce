var builder = WebApplication.CreateBuilder(args);
{
    ConfigurationManager configuration = builder.Configuration;

    // Add services to the container.
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddHttpContextAccessor();

}

var app = builder.Build();
{
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
    }

    app.UseCors("corsapp");
    app.UseExceptionHandler("/error");
    app.UseHttpsRedirection();
    app.MapControllers();
    
    app.Run();
}

public partial class Program { }