using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using Account.Application.Dtos;
using Account.Core.User;
using Account.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Account.IntegrationTest;

public class AccountIntegrationTest : IClassFixture<CustomWebApplicationFactory<Program>>, IDisposable
{
    private readonly CustomWebApplicationFactory<Program> _factory;

    public AccountIntegrationTest(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        
        using (var scope = _factory.Services.CreateScope())
        {
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<DataContext>();
            
            db.Customers.Add(Customer.Create("tester@gmx.de","testPassword"));
            db.SaveChanges();
        }
    }

    public void Dispose()
    {
        using (var scope = _factory.Services.CreateScope())
        {
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<DataContext>();

            db.Customers.RemoveRange(db.Customers);
            db.SaveChanges();
        }
    }

    [Fact]
    public async Task SuccessfullyLogsIn()
    {
        // Arrange
        var client = _factory.CreateClient();
        var content = new StringContent("", Encoding.UTF8, "application/json");
        var email = "tester@gmx.de";
        var password = "testPassword";
        
        // Act
        var response = await client.PostAsync($"/api/Authentication/login?email={email}&password={password}", content);
        // Assert
        response.EnsureSuccessStatusCode();
        var token = await response.Content.ReadAsStringAsync();
        
        var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
        Assert.NotNull(token);
        Assert.Equal(email, jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value);
    }

    [Fact]
    public async Task BadRequest_When_UserDoes_Not_Exist()
    {
        // Arrange
        var client = _factory.CreateClient();
        var content = new StringContent("", Encoding.UTF8, "application/json");
        var email = "noUserFound@gmx.de";
        var password = "testPassword";

        // Act
        var response = await client.PostAsync($"/api/Authentication/login?email={email}&password={password}", content);
        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal("Invalid login", await response.Content.ReadAsStringAsync());
    }
    
    [Fact]
    public async Task User_Is_Persisted_On_Registration()
    {
        // Arrange
        var client = _factory.CreateClient();
        var content = new StringContent(
            @"{
                ""Email"": ""newRegistered@gmx.de"",
                ""Password"": ""testPassword""
            }",
            Encoding.UTF8,
            "application/json");

        // Act
        var response = await client.PostAsync($"/api/Authentication/register", content);

        // Assert
        response.EnsureSuccessStatusCode();
        var customer = await response.Content.ReadFromJsonAsync<Customer>();
        Assert.NotNull(customer);
        
        // check db creation
        using (var scope = _factory.Services.CreateScope())
        {
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<DataContext>();
            
            var persistedCustomer = db.Customers.FirstOrDefault(c => c.Email == "newRegistered@gmx.de");
            Assert.NotNull(persistedCustomer);
        }
    }

    
    [Fact]
    public async Task BadRequest_When_Email_Already_Exists()
    {
        // Arrange
        var client = _factory.CreateClient();
        var content = new StringContent(
            @"{
                ""Email"": ""tester@gmx.de"",
                ""Password"": ""testPassword""
            }", Encoding.UTF8, "application/json");

        // Act
        var response = client.PostAsync($"/api/Authentication/register", content);
        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.Result.StatusCode);
        Assert.Equal("Email already exists", await response.Result.Content.ReadAsStringAsync());
    }
    
        [Fact]
    public async Task Unauthorized_When_User_Is_Not_Authenticated()
    {
        // Arrange
        var client = _factory.CreateClient();
        // Act
        var response = await client.GetAsync($"/api/CustomerProfiles");
        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Returns_CustomerProfile_When_User_Is_Authenticated()
    {
        // Arrange
        var client = _factory.CreateClient();
        var token = await ObtainJwtToken();
        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        // Act
        var response = await client.GetAsync($"/api/CustomerProfiles");
        // Assert
        response.EnsureSuccessStatusCode();
        var customerProfile = await response.Content.ReadFromJsonAsync<CustomerResponseDto>();
        Assert.NotNull(customerProfile);
    }

    
    [Fact]
    public async Task Update_Request_Is_Persisted_To_Database()
    {
        // Arrange
        var client = _factory.CreateClient();
        var token = await ObtainJwtToken();
        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        var content = new StringContent(
            @"{
                ""address"": {
                    ""street"": ""steet"",
                    ""city"": ""city"",
                    ""zip"": ""12345"",
                    ""country"": ""Germany""
                },
                ""personalInformation"": {
                    ""firstName"": ""tester"",
                    ""lastName"": ""test"",
                    ""dateOfBirth"": ""24.06.1998""
                },
                ""paymentInformation"": {
                ""address"": {
                    ""street"": ""street"",
                    ""city"": ""city"",
                    ""zip"": ""12345"",
                    ""country"": ""Germany""
                }
            }
        }",
            Encoding.UTF8,
            "application/json");
        
        // Act
        var response = await client.PutAsync($"/api/CustomerProfiles", content);
        
        // Assert
        response.EnsureSuccessStatusCode();
        
        using (var scope = _factory.Services.CreateScope())
        {
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<DataContext>();
            var customer = db.Customers.FirstOrDefault();
            Assert.NotNull(customer);
            Assert.Equal("tester", customer.PersonalInformation.FirstName);
            Assert.Equal("test", customer.PersonalInformation.LastName);
            Assert.Equal(DateOnly.Parse("24.06.1998"), customer.PersonalInformation.DateOfBirth);
            Assert.Equal("steet", customer.Address.Street);
            Assert.Equal("city", customer.Address.City);
            Assert.Equal("12345", customer.Address.Zip);
            Assert.Equal("Germany", customer.Address.Country);
           // Assert.Equal("steet", customer.PaymentInformation.Address.Street);
            Assert.Equal("city", customer.PaymentInformation.Address.City);
            Assert.Equal("12345", customer.PaymentInformation.Address.Zip);
            Assert.Equal("Germany", customer.PaymentInformation.Address.Country);
        }
    }
    
    [Fact]
    public async Task Delelte_Request_Is_Persisted_To_Database()
    {
        // Arrange
        var client = _factory.CreateClient();
        var token = await ObtainJwtToken();
        client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        
        // Act
        var response = await client.DeleteAsync($"/api/CustomerProfiles");
        
        // Assert
        response.EnsureSuccessStatusCode();
        
        using (var scope = _factory.Services.CreateScope())
        {
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<DataContext>();
            var customer = db.Customers.FirstOrDefault();
            Assert.Null(customer);
        }
    }

    private async Task<string> ObtainJwtToken()
    {
        var client = _factory.CreateClient();
        var content = new StringContent("", Encoding.UTF8, "application/json");
        var email = "tester@gmx.de";
        var password = "testPassword";

        var response = await client.PostAsync($"/api/Authentication/login?email={email}&password={password}", content);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
}



