using Account.Application.Dtos;

namespace Account.Application.Profile;

public interface IProfileService
{
    Task<CustomerResponseDto> GetProfile(Guid customerId);
    Task UpdateProfile(Guid customerId, CustomerUpdateDto customerUpdateDto);
    Task DeleteProfile(Guid customerId);
}