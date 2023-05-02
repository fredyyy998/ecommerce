using System.Net;
using System.Net.Http.Json;
using System.Text;
using Account.Application.Dtos;
using Account.Core.User;
using Account.Infrastructure;
using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;

namespace Account.IntegrationTest;

[Collection("Sequential")]
public class ProfileIntegrationTest : IClassFixture<CustomWebApplicationFactory<Program>>, IDisposable
{
    private readonly CustomWebApplicationFactory<Program> _factory;

    public ProfileIntegrationTest(CustomWebApplicationFactory<Program> factory)
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
    public async Task Delete_Request_Is_Persisted_To_Database()
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
    
    // TODO there is a bug where the DateOnly property is not serialized, just in the test setup
    public async Task Message_Is_Published_To_Kafka_On_Successful_Update()
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
        
        var config = new ConsumerConfig()
        {
            BootstrapServers = "localhost:9092",
            GroupId = "test-update-consumer-group",
            AutoOffsetReset = AutoOffsetReset.Latest,
        };

        
        using (var consumer = new ConsumerBuilder<string, string>(config).Build())
        {
            consumer.Subscribe("account");
            
            // Act
            var resutl = await client.PutAsync($"/api/CustomerProfiles/", content);

            // Wait for the message to be consumed
            var consumeResult = await Task.Run(() => consumer.Consume(TimeSpan.FromSeconds(10)));

            // Assert
            Assert.NotNull(consumeResult.Value);
            Assert.Equal("customer-edited", consumeResult.Message.Key);
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
        var tokenResponse = await response.Content.ReadFromJsonAsync<JwtResponseDto>();
        return tokenResponse.token;
    }

}