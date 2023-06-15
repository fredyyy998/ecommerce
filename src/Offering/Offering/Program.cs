

var builder = WebApplication.CreateBuilder(args);
{
    ConfigurationManager configuration = builder.Configuration;
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddHttpContextAccessor();
}

var app = builder.Build();
{
    app.UseExceptionHandler("/error");
    app.UseHttpsRedirection();
    app.MapControllers();
    
    app.Run();
}