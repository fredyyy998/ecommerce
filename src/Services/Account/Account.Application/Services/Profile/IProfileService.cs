using Account.Application.Dtos;

namespace Account.Application.Profile;

public interface IProfileService
{
    CustomerResponseDto GetProfile(Guid customerId);
    void UpdateProfile(Guid customerId, CustomerUpdateDto customerUpdateDto);
    void DeleteProfile(Guid customerId);
}