using Account.Core.User;
using Ecommerce.Common.Core;

namespace Account.Core.Events;

public class CustomerRegisteredEvent : IDomainEvent
{
    public Guid CustomerId { get; }
    public string Email { get; }
    public DateTime CreatedAt { get;  }

    public CustomerRegisteredEvent(Customer customer)
    {
        CustomerId = customer.Id;
        Email = customer.Email;
        CreatedAt = customer.CreatedAt;
    }
}