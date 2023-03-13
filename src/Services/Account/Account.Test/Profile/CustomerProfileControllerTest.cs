using System.Security.Claims;
using Account.Application.Dtos;
using Account.Application.Exceptions;
using Account.Application.Profile;
using Account.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Account.Test.Profile;

public class CustomerProfileControllerTest
{
    private Mock<IProfileService> _profileServiceMock;

    private CustomerProfilesController _customerProfilesController;

    public CustomerProfileControllerTest()
    {
        _profileServiceMock = new Mock<IProfileService>();
        _customerProfilesController = new CustomerProfilesController(_profileServiceMock.Object);
    }

    [Fact]
    public void GetProfile_WhenCalled_With_Set_Token_ReturnsOk()
    {
        var customerId = Guid.NewGuid();
        var customer = new CustomerResponseDto(customerId, "test@mail.de", null, null, null);
        _profileServiceMock.Setup(x => x.GetProfile(customerId)).Returns(customer);
        SetupHttpContextWithCustomerSub(customerId);

        var result = _customerProfilesController.GetProfile();
        
        Assert.IsType<OkObjectResult>(result);
        Assert.Equal(customer, ((OkObjectResult)result).Value);
    }

    [Fact]
    public void GetProfile_With_not_existing_user_returns_404()
    {
        var customerId = Guid.NewGuid();
        var customer = new CustomerResponseDto(customerId, "test@mail.de", null, null, null);
        _profileServiceMock.Setup(x => x.GetProfile(customerId)).Throws(new EntityNotFoundException("Customer not found"));
        SetupHttpContextWithCustomerSub(customerId);

        var result = _customerProfilesController.GetProfile();
        
        Assert.IsType<NotFoundObjectResult>(result);
    }
    
    [Fact]
    public void GetProfile_with_unset_token_returns_Unauthorized()
    {
        SetupHttpContext(new ClaimsIdentity());
        
        var result = _customerProfilesController.GetProfile();
        
        Assert.IsType<UnauthorizedObjectResult>(result);
    }

    private void SetupHttpContextWithCustomerSub(Guid customerId)
    {
        // Create a claim
        var claim = new Claim(ClaimTypes.NameIdentifier, customerId.ToString());

        // Create a ClaimsIdentity with the claim
        var claimsIdentity = new ClaimsIdentity(new[] { claim }, "TestAuthentication");
        SetupHttpContext(claimsIdentity);
    }
    
    private void SetupHttpContext(ClaimsIdentity claimsIdentity)
    {
        // Create a ClaimsPrincipal with the ClaimsIdentity
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        // Set the ClaimsPrincipal on the HttpContext
        var httpContext = new DefaultHttpContext();
        httpContext.User = claimsPrincipal;
        _customerProfilesController.ControllerContext.HttpContext = httpContext;
    }
}