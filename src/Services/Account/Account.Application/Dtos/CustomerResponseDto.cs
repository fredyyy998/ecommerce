using System.ComponentModel.DataAnnotations;

namespace Account.Application.Dtos;

public class CustomerResponseDto
{
    public Guid Id { get; set; }
    
    [EmailAddress]
    public string Email { get; set; }
    public AddressDto? Address { get; set; }
    public PersonalInformationDto? PersonalInformation { get; set; }
    
    public PaymentInformationDto? PaymentInformation { get; set; }
    
    public CustomerResponseDto(
        Guid id,
        string email,
        AddressDto? address,
        PersonalInformationDto? personalInformation,
        PaymentInformationDto? paymentInformation)
    {
        Id = id;
        Email = email;
        Address = address;
        PersonalInformation = personalInformation;
        PaymentInformation = paymentInformation;
    }
}
