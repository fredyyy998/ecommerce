using Ecommerce.Common.Core;

namespace Fulfillment.Core.DomainEvents;

public class CustomerRegisteredEvent : IDomainEvent
{
    public Guid CustomerId { get; }
    public string Email { get; }
    public DateTime CreatedAt { get;  }

    public CustomerRegisteredEvent(Guid customerId, string email, DateTime createdAt)
    {
        CustomerId = customerId;
        Email = email;
        CreatedAt = createdAt;
    }
}