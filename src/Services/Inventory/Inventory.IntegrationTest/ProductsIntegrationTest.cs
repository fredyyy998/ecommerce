using System.Net;
using System.Net.Http.Json;
using Inventory.Application.Dtos;
using Inventory.Core.Product;
using Inventory.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Inventory.IntegrationTest;

[Collection("Sequential")]
public class ProductsIntegrationTest : IClassFixture<CustomWebApplicationFactory<Program>>, IDisposable
{
    private readonly CustomWebApplicationFactory<Program> _factory;

    public ProductsIntegrationTest(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        
        SeedDatabase(factory);
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
}