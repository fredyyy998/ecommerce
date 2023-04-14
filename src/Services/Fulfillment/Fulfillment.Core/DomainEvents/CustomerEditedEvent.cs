using Ecommerce.Common.Core;
using Fulfillment.Core.Buyer;

namespace Fulfillment.Core.DomainEvents;

public class CustomerEditedEvent : IDomainEvent
{
    public Guid CustomerId { get; }
    public string Email { get; }
    public Address Address { get; }
    public PersonalInformationDto PersonalInformation { get; }
    public PaymentInformation PaymentInformation { get; }
    public DateTime CreatedAt { get;  }
    public DateTime UpdatedAt { get; }
    
    public CustomerEditedEvent(Guid customerId, string email, Address address, PersonalInformationDto personalInformation, PaymentInformation paymentInformation, DateTime createdAt, DateTime updatedAt)
    {
        CustomerId = customerId;
        Email = email;
        Address = address;
        PersonalInformation = personalInformation;
        PaymentInformation = paymentInformation;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }
}

public class PersonalInformationDto {
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateOnly DateOfBirth { get; set; }
}
