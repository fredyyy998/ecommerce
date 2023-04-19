using Account.Application.Dtos;

namespace Account.Application.Profile;

public interface IAuthenticationService
{
    Task<CustomerResponseDto> RegisterCustomer(CustomerCreateDto userDto);
    Task<JwtResponseDto> AuthenticateUser(string email, string password);
}