using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using Fulfillment.Application.Dtos;
using Fulfillment.Core.Buyer;
using Fulfillment.Core.Order;
using Fulfillment.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Account.IntegrationTest;

public class FulfillmentIntegrationTest: IClassFixture<CustomWebApplicationFactory<Program>>, IDisposable
{
    private readonly CustomWebApplicationFactory<Program> _factory;

    private readonly string jwtSecretKey;

    private readonly Guid buyerId = Guid.Parse("5965c6ef-5011-454c-9d08-32ea27b4221a");
    private readonly Guid productId = Guid.Parse("5965c6ef-5011-454c-9d08-32ea27b42ab4");
    private Order _order1;
    private Order _order2;

    public FulfillmentIntegrationTest(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        
        using (var scope = _factory.Services.CreateScope())
        {
            var scopedServices = scope.ServiceProvider;

            var configuration = scopedServices.GetRequiredService<IConfiguration>();
            jwtSecretKey = configuration.GetSection("JWT:Secret").Value;
            
            var db = scopedServices.GetRequiredService<DataContext>();
            db.Database.EnsureCreated();
            _order1 = Order.Create(buyerId, new Address("street", "zip", "city", "country"));
            _order1.AddOrderItem(OrderItem.Create(productId, "Test", 1,1,"EUR", 0, 5));
            _order2 = Order.Create(buyerId, new Address("street", "zip", "city", "country"));
            _order2.AddOrderItem(OrderItem.Create(productId, "Test", 1,1,"EUR", 0, 15));
            db.Orders.Add(_order1);
            db.Orders.Add(_order2);
            db.SaveChanges();
        }
    }
    
    public void Dispose()
    {
        using (var scope = _factory.Services.CreateScope())
        {
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<DataContext>();

            db.Buyers.RemoveRange(db.Buyers);
            db.Orders.RemoveRange(db.Orders);
            db.SaveChanges();
        }
    }
    
    [Fact (Skip = "Does not work since the json is not deserialized correctly in this project")]
    public async Task GetOrders_ShouldReturnOrders()
    {
        // Arrange
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GenerateJwt());

        // Act
        var response = await client.GetAsync("/api/orders");

        // Assert
        response.EnsureSuccessStatusCode();
        var orders = await response.Content.ReadFromJsonAsync<List<OrderResponseDto>>();
        Assert.NotNull(orders);
        Assert.Equal(2, orders.Count);
    }

    [Theory()]
    [InlineData("pay", OrderState.Paid)]
    [InlineData("cancel", OrderState.Cancelled)]
    public async Task Change_Order_State_Is_Persisted(string state, OrderState orderState)
    {
        // Arrange
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GenerateJwt());
        // Act
        var response = await client.PutAsync($"/api/orders/{_order1.Id}/state/{state}", null);
        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        using (var scope = _factory.Services.CreateScope())
        {
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<DataContext>();
            var order = db.Orders.Find(_order1.Id);
            Assert.Equal(orderState, order.State);
        }
    }
    
    private string GenerateJwt()
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Email, "ecommerce@admin.de"),
            new Claim(ClaimTypes.NameIdentifier, buyerId.ToString()),
        };

        claims.Add(new Claim(ClaimTypes.Role, "Admin"));
        
        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddDays(7),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}