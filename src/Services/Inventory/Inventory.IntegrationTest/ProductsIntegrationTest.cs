using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using Inventory.Application.Dtos;
using Inventory.Core.Product;
using Inventory.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Inventory.IntegrationTest;

[Collection("Sequential")]
public class ProductsIntegrationTest : IClassFixture<CustomWebApplicationFactory<Program>>, IDisposable
{
    private readonly CustomWebApplicationFactory<Program> _factory;
    private readonly string jwtSecretKey;

    public ProductsIntegrationTest(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        
        SeedDatabase(factory);
        using (var scope = factory.Services.CreateScope())
        {
            var scopedServices = scope.ServiceProvider;
            var configuration = scopedServices.GetRequiredService<IConfiguration>();
            jwtSecretKey = configuration.GetSection("JWT:Secret").Value;
        }
    }

    async private static void SeedDatabase(CustomWebApplicationFactory<Program> factory)
    {
        using (var scope = factory.Services.CreateScope())
        {
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<DataContext>();
            
            var product1 = Product.Create("testProduct", "testDescription", 10.0m);
            var product2 = Product.Create("otherProduct", "testDescription", 20.0m);
            var product3 = Product.Create("notStockproduct", "testDescription", 20.0m);
            product1.AddStock(10);
            product2.AddStock(20);
            
            db.Products.Add(product1);
            db.Products.Add(product2);
            db.Products.Add(product3);
            await db.SaveChangesAsync();
        }
    }
    
    public async void Dispose()
    {
        using (var scope = _factory.Services.CreateScope())
        {
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<DataContext>();

            db.Products.RemoveRange(db.Products);
            await db.SaveChangesAsync();
        }
    }
    
    
    [Fact()]
    public async Task Products_In_Sock_Are_Returned_As_List()
    {
        // Arrange
        var client = _factory.CreateClient();
        // Act
        var response = await client.GetAsync($"/api/Products");
        // Assert
        var products = await response.Content.ReadFromJsonAsync<List<ProductResponseDto>>();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(2, products.Count);
    }

    [Fact()]
    public async Task ProductManagement_Returns_All_Products()
    {
        // Arrange
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GenerateJwt());
        // Act
        var response = await client.GetAsync($"/api/ProductManagement");
        // Assert
        Assert.True(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
        var products = await response.Content.ReadFromJsonAsync<List<AdminProductResponseDto>>();
        Assert.Equal(3, products.Count);
    }

    private string GenerateJwt()
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Email, "ecommerce@admin.de"),
            new Claim(ClaimTypes.NameIdentifier, "1234"),
        };

        claims.Add(new Claim(ClaimTypes.Role, "Admin"));
        
        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddDays(7),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}