using Account.Application.Dtos;
using Account.Core.User;

namespace Account.Application;

public class MappingProfile : AutoMapper.Profile
{
    public MappingProfile()
    {
        CreateMap<Customer, CustomerResponseDto>();
        CreateMap<Address, AddressDto>();
        CreateMap<PaymentInformation, PaymentInformationDto>();
        CreateMap<PersonalInformation, PersonalInformationDto>();
    }
}