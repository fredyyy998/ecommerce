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
using Newtonsoft.Json;

namespace Inventory.IntegrationTest;

[Collection("Sequential")]
public class ProductsIntegrationTest : IClassFixture<CustomWebApplicationFactory<Program>>, IDisposable
{
    private readonly CustomWebApplicationFactory<Program> _factory;
    private readonly string jwtSecretKey;
    
    private Product product1;
    private Product product2;
    private Product product3;

    public ProductsIntegrationTest(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        
        product1 = Product.Create("testProduct", "testDescription", 10.0m);
        product2 = Product.Create("otherProduct", "testDescription", 20.0m);
        product3 = Product.Create("notStockproduct", "testDescription", 20.0m);
        product1.AddStock(10);
        product2.AddStock(20);

        
        SeedDatabase(factory, product1, product2, product3);
        using (var scope = factory.Services.CreateScope())
        {
            var scopedServices = scope.ServiceProvider;
            var configuration = scopedServices.GetRequiredService<IConfiguration>();
            jwtSecretKey = configuration.GetSection("JWT:Secret").Value;
        }
    }

    async private static void SeedDatabase(CustomWebApplicationFactory<Program> factory, Product product1, Product product2, Product product3)
    {
        using (var scope = factory.Services.CreateScope())
        {
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<DataContext>();
            
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
            db.SaveChanges();
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
    
    [Fact()]
    public async Task ProductManagement_Returns_Unauthorized_Without_Jwt()
    {
        // Arrange
        var client = _factory.CreateClient();
        // Act
        var response = await client.GetAsync($"/api/ProductManagement");
        // Assert
        Assert.False(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact()]
    public async Task Added_Product_Is_Persisted()
    {
        // Arrange
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GenerateJwt());
        // Act
        var response = await client.PostAsJsonAsync($"/api/ProductManagement", new ProductCreateDto(
            "newlyAddedProduct",
            "newDescription",
            10.0m));
        // Assert
        Assert.True(response.IsSuccessStatusCode);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        
        // check db creation
        using (var scope = _factory.Services.CreateScope())
        {
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<DataContext>();
            
            var persistedProduct = db.Products.FirstOrDefault(c => c.Name == "newlyAddedProduct");
            Assert.NotNull(persistedProduct);
        }
    }
    
    [Fact()]
    public async Task Added_Product_Without_Jwt_Is_Unauthorized()
    {
        // Arrange
        var client = _factory.CreateClient();
        // Act
        var response = await client.PostAsJsonAsync($"/api/ProductManagement", new ProductCreateDto(
            "newlyAddedProduct",
            "newDescription",
            14.0m));
        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact(Skip = "Missing functionality in test library, no Patch request possible")] 
    public async Task Update_To_Product_Is_Persisted()
    {
        // arrange
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GenerateJwt());
        // Act
        var patchData =JsonConvert.SerializeObject(new ProductUpdateDto(
            "UpdatedName",
            "description",
            14.0m,
            new List<KeyValuePair<string, string>>()));
        // This method is not working, because the real endpoint is patch, but there is no way to call patchAsJsonAsync
        var response = await client.PutAsJsonAsync($"/api/ProductManagement/{product1.Id}", patchData);
        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        
        // check db creation
        using (var scope = _factory.Services.CreateScope())
        {
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<DataContext>();
            
            var persistedProduct = db.Products.FirstOrDefault(c => c.Id == product1.Id);
            Assert.Equal(persistedProduct.Name, "UpdatedName");
            Assert.Equal(persistedProduct.Description, "description");
            Assert.Equal(persistedProduct.Price.GrossPrice, 14.0m);
        }
    }

    [Fact()]
    public async Task Delete_Is_Persisted()
    {
        // arrange
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GenerateJwt());
        // act
        var response = await client.DeleteAsync($"/api/ProductManagement/{product1.Id}");
        // assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        using (var scope = _factory.Services.CreateScope())
        {
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<DataContext>();

            var persistedProduct = db.Products.FirstOrDefault(c => c.Id == product1.Id);
            Assert.Null(persistedProduct);
        }
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