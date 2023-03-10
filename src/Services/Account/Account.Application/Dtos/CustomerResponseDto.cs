namespace Account.Application.Dtos;

public record CustomerResponseDto(
    Guid Id,
    string Email,
    AddressDto? Address,
    PersonalInformationDto? PersonalInformation,
    PaymentInformationDto? PaymentInformation);