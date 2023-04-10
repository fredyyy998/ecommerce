using Account.Core.User;
using Ecommerce.Common.Core;

namespace Account.Core.Events;

public class CustomerEditedEvent : IDomainEvent
{
    public Guid CustomerId { get; }
    public string Email { get; }
    public Address Address { get; }
    public PersonalInformation PersonalInformation { get; }
    public PaymentInformation PaymentInformation { get; }
    public DateTime CreatedAt { get;  }
    public DateTime UpdatedAt { get; }
    
    public CustomerEditedEvent(Customer customer)
    {
        CustomerId = customer.Id;
        Email = customer.Email;
        Address = customer.Address;
        PersonalInformation = customer.PersonalInformation;
        PaymentInformation = customer.PaymentInformation;
        CreatedAt = customer.CreatedAt;
        UpdatedAt = customer.UpdatedAt;
    }
}