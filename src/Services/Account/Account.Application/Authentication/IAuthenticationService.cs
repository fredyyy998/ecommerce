using System.Security.Claims;
using Account.Application.Dtos;

namespace Account.Application.Profile;

public interface IAuthenticationService
{
    CustomerResponseDto RegisterCustomer(CustomerCreateDto userDto);
    string AuthenticateCustomer(string email, string password);
    ClaimsPrincipal ValidateToken(string token);
}