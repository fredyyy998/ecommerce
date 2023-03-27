using Account.Application.Dtos;

namespace Account.Application.Profile;

public interface IAuthenticationService
{
    CustomerResponseDto RegisterCustomer(CustomerCreateDto userDto);
    string AuthenticateUser(string email, string password);
}