using System.Net;
using System.Net.Http.Json;
using Account.Application.Dtos;
using Account.Core.User;
using Account.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Account.Test;

// TODO these integration tests are not getting setup correctly, they will be running on
// TODO the real database, not the in memory database
// TODO Also the Integration test should be moved in an own project to split them from the unit tests
public class AuthenticationTest : IClassFixture<BaseWebApplicationFactory<Program>>
{
    private readonly BaseWebApplicationFactory<Program> _factory;

    public AuthenticationTest(BaseWebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }
    
    public async Task RegisterCustomer_ValidInput_ReturnsCreatedStatusCode()
    {
        // Arrange
        var client = _factory.CreateClient();
        var request = new CustomerCreateDto( "test@example.com", "Password1!");

        // Act
        var response = await client.PostAsJsonAsync("/api/Authentication/register", request);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    
    public async Task RegisterCustomer_DuplicateEmail_ReturnsBadRequestStatusCode()
    {
        // Arrange
        var client = _factory.CreateClient();
        var request = new CustomerCreateDto( "test@example.com", "Password1!");

        // Ensure a customer with the same email already exists
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
        dbContext.Customers.Add(Customer.Create(request.Email, request.Password));
        await dbContext.SaveChangesAsync();

        // Act
        var response = await client.PostAsJsonAsync("/api/customers", request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}