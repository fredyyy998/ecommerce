using Account.Application.Dtos;
using Account.Application.Exceptions;
using Account.Application.Profile;
using Account.Web;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ValidationException = FluentValidation.ValidationException;

namespace Account.Test.Profile;

public class AuthenticationControllerTest
{
    private AuthenticationController _authenticationController;
    
    private Mock<IAuthenticationService> _authenticationServiceMock;
    
    public AuthenticationControllerTest()
    {
        _authenticationServiceMock = new Mock<IAuthenticationService>();
        _authenticationController = new AuthenticationController(_authenticationServiceMock.Object);
    }
    
    [Fact]
    public void Successful_register_returns_customer()
    {
        var guid = Guid.Parse("00000000-0000-0000-0000-000000000000");
        var email = "test@customer.de";
        var password = "abc123";
        var customerCreateDto = new CustomerCreateDto(email, password);
        var customerResponseDto = new CustomerResponseDto(guid, email, null, null, null);
        _authenticationServiceMock
            .Setup(x => x.RegisterCustomer(It.IsAny<CustomerCreateDto>()))
            .Returns(customerResponseDto);
        
        var result = _authenticationController.RegisterCustomer(customerCreateDto);
        
        Assert.IsType<OkObjectResult>(result);
    }

    [Theory]
    [InlineData(typeof(EmailAlreadyExistsException), "Email already exists")]
    [InlineData(typeof(ValidationException), "Invalid data")]
    public void Register_on_invalid_data_response_with_bad_request(Type exceptionType, string exceptionMessage)
    {
        var email = "test@customer.de";
        var password = "abc123";
        var customerCreateDto = new CustomerCreateDto(email, password);
        _authenticationServiceMock
            .Setup(x => x.RegisterCustomer(It.IsAny<CustomerCreateDto>()))
            .Throws((Exception)Activator.CreateInstance(exceptionType, exceptionMessage));

        var result = _authenticationController.RegisterCustomer(customerCreateDto);
        
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public void Valid_login_will_respond_with_token()
    {
        var email = "test@customer.de";
        var password = "abc123";
        var token = "token";
        _authenticationServiceMock.Setup(x => x.AuthenticateUser(email, password))
            .Returns(token);

        var result = _authenticationController.LoginCustomer(email, password);
        
        Assert.IsType<OkObjectResult>(result);
        Assert.Equivalent(new { token = token }, ((OkObjectResult)result).Value);
    }

    [Fact]
    public void Invalid_login_will_respond_with_bad_request()
    {
        var email = "test@customer.de";
        var password = "abc123";
        _authenticationServiceMock.Setup(x => x.AuthenticateUser(email, password))
            .Throws(new InvalidLoginException("Invalid login"));
        
        var result = _authenticationController.LoginCustomer(email, password);
        
        Assert.IsType<BadRequestObjectResult>(result);
    }
}