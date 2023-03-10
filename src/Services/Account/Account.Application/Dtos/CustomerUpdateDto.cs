using Account.Core.User;

namespace Account.Application.Dtos;

public record CustomerUpdateDto(
    AddressDto Address,
    PersonalInformationDto PersonalInformation,
    PaymentInformationDto? PaymentInformation);
