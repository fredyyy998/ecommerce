using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Newtonsoft.Json;
using ShoppingCart.Application.Dtos;
using ShoppingCart.Core.Product;
using ShoppingCart.Infrastructure;

namespace Account.IntegrationTest;

public class ShoppingCartIntegrationTest : IClassFixture<CustomWebApplicationFactory<Program>>, IDisposable
{
    private readonly CustomWebApplicationFactory<Program> _factory;

    private readonly string jwtSecretKey;

    private readonly Guid customerGuid = Guid.Parse("5965c6ef-5011-454c-9d08-32ea27b4221a");

    private Guid product1Guid = Guid.Parse("9c9682d9-275d-44cd-bd25-c6332b405eeb");
    private Guid product2Guid = Guid.Parse("c1157e0f-8c36-4ad9-a67a-a3e4703a42e2");
    private Guid product3Guid = Guid.Parse("2ceb1729-a181-4313-9c21-9758c3b26844");
    private Product _product1;
    private Product _product2;
    private Product _product3;

    public ShoppingCartIntegrationTest(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        
        using (var scope = _factory.Services.CreateScope())
        {
            var scopedServices = scope.ServiceProvider;

            var configuration = scopedServices.GetRequiredService<IConfiguration>();
            jwtSecretKey = configuration.GetSection("JWT:Secret").Value;

            var db = scopedServices.GetRequiredService<DataContext>();
            
            _product1 = Product.Create(product1Guid, "TestProduct1", "TestDescription1", new Price(81, 100, "EUR"), 20);
            _product2 = Product.Create(product2Guid, "TestProduct2", "TestDescription2", new Price(81, 100, "EUR"), 20);
            _product3 = Product.Create(product3Guid, "TestProduct3", "TestDescription3", new Price(81, 100, "EUR"), 20);
            db.Products.Add(_product1);
            db.Products.Add(_product2);
            db.Products.Add(_product3);
            db.SaveChanges();
        }
    }
    
    public void Dispose()
    {
        using (var scope = _factory.Services.CreateScope())
        {
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<DataContext>();

            db.Products.RemoveRange(db.Products);
            db.ShoppingCarts.RemoveRange(db.ShoppingCarts);
            db.SaveChanges();
        }
    }

    [Fact]
    public async Task First_Product_Of_Customer_Creates_Shopping_Basket()
    {
        // Arrange
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GenerateJwt());
        // Act
        var response = await client.PutAsJsonAsync($"/api/ShoppingCart/items/{product1Guid}", new QuantityRequestDto(2));
        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        using (var scope = _factory.Services.CreateScope())
        {
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<DataContext>();
            
            var persistedShoppingCart = db.ShoppingCarts.FirstOrDefault(c => c.CustomerId == customerGuid);
            Assert.NotNull(persistedShoppingCart);
            Assert.Equal(persistedShoppingCart.Items.Count, 1);
            Assert.Equal(persistedShoppingCart.Items.First().Quantity, 2);
        }
    }

    [Fact]
    public async Task Customer_Without_Shopping_Basket_Gets_No_Content()
    {
        // Arrange
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GenerateJwt());
        // Act
        var response = await client.GetAsync($"/api/ShoppingCart");
        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }
    
    [Fact]
    public async Task Customer_With_Shopping_Basket_Gets_Ok()
    {
        // Arrange
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GenerateJwt());
        await client.PutAsJsonAsync($"/api/ShoppingCart/items/{product1Guid}", new QuantityRequestDto(2));
        // Act
        var response = await client.GetAsync($"/api/ShoppingCart");
        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var result = response.Content.ReadFromJsonAsync<ShoppingCartResponseDto>().Result;
        Assert.NotNull(result);
        Assert.Equal(result.Items.Count(), 1);
        Assert.Equal(result.Items.First().ProductId, product1Guid);
    }

    [Fact]
    public async Task Customer_Adds_Not_Existing_Product_Returns_Error()
    {
        // Arrange
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GenerateJwt());
        // act
        var guid = Guid.Parse("9c9682d9-275d-44cd-bd25-c6332b405eee");
        var response = await client.PutAsJsonAsync($"/api/ShoppingCart/items/{guid}", new QuantityRequestDto(1));
        // assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Customer_Adds_More_Products_Than_In_Stock_Returns_Error()
    {
        // Arrange
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GenerateJwt());
        // act
        var response = await client.PutAsJsonAsync($"/api/ShoppingCart/items/{product1Guid}", new QuantityRequestDto(21));
        // assert
        var content = await response.Content.ReadAsStringAsync();
        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
    }

    [Fact]
    public async Task Customer_Removes_Item_From_Cart_Is_Persisted()
    {
        // Arrange
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GenerateJwt());
        // TODO in general it would be better to do this directly at the db, but unfortunately it will throw an error
        await client.PutAsJsonAsync($"/api/ShoppingCart/items/{product1Guid}", new QuantityRequestDto(2));
        // act
        var response = await client.DeleteAsync($"/api/ShoppingCart/items/{product1Guid}");
        // assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        using (var scope = _factory.Services.CreateScope())
        {
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<DataContext>();
            
            var persistedShoppingCart = db.ShoppingCarts.FirstOrDefault(c => c.CustomerId == customerGuid);
            Assert.NotNull(persistedShoppingCart);
            Assert.Equal(persistedShoppingCart.Items.Count, 0);
        }
    }
    
    [Fact]
    public async Task Customer_Removes_Item_From_Cart_That_Does_Not_Exist_Returns_Error()
    {
        // Arrange
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GenerateJwt());
        // act
        var guid = Guid.Parse("9c9682d9-275d-44cd-bd25-c6332b405eee");
        var response = await client.DeleteAsync($"/api/ShoppingCart/items/{guid}");
        // assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact(Skip = "There is an test triggers a db error, that does not occur in the normal startup")]
    public async Task Customer_Creates_Checkout_Is_Persisted()
    {
        // arrange
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GenerateJwt());
        await client.PutAsJsonAsync($"/api/ShoppingCart/items/{product1Guid}", new QuantityRequestDto(2));
        // act
        var requestBody = new CheckoutRequestDto
        {
            Email = "customer@email.com",
            FirstName = "Max",
            LastName = "Mustermann",
            CustomerId = Guid.Parse("9c9682d9-275d-44cd-bd25-c6332b405acc"),
            ShippingAddress = new AddressDto("Musterstraße 1", "12345", "Musterstadt", "Deutschland"),
        };
        var serializedRequest = JsonConvert.SerializeObject(requestBody);
        var requestContent = new StringContent(serializedRequest, Encoding.UTF8, "application/json");
        var response = await client.PatchAsync("/api/ShoppingCart/state/checkout", requestContent);
        // assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        using (var scope = _factory.Services.CreateScope())
        {
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<DataContext>();
            
            var persistedShoppingCart = db.ShoppingCarts.FirstOrDefault(c => c.CustomerId == customerGuid);
            Assert.NotNull(persistedShoppingCart);
            Assert.Equal(persistedShoppingCart.ShoppingCartCheckout.Email, requestBody.Email);
            Assert.Equal(persistedShoppingCart.ShoppingCartCheckout.FirstName, requestBody.FirstName);
            Assert.Equal(persistedShoppingCart.ShoppingCartCheckout.LastName, requestBody.LastName);
            Assert.Equal(persistedShoppingCart.ShoppingCartCheckout.CustomerId, requestBody.CustomerId);
            Assert.Equal(persistedShoppingCart.ShoppingCartCheckout.ShippingAddress.Street, requestBody.ShippingAddress.Street);
            Assert.Equal(persistedShoppingCart.ShoppingCartCheckout.ShippingAddress.ZipCode, requestBody.ShippingAddress.ZipCode);
            Assert.Equal(persistedShoppingCart.ShoppingCartCheckout.ShippingAddress.City, requestBody.ShippingAddress.City);
            Assert.Equal(persistedShoppingCart.ShoppingCartCheckout.ShippingAddress.Country, requestBody.ShippingAddress.Country);
        }
    }

    private string GenerateJwt()
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Email, "ecommerce@admin.de"),
            new Claim(ClaimTypes.NameIdentifier, customerGuid.ToString()),
        };

        claims.Add(new Claim(ClaimTypes.Role, "Admin"));
        
        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddDays(7),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}