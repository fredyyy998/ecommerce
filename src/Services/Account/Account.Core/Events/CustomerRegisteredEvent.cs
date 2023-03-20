using Account.Core.User;
using Ecommerce.Common.Core;

namespace Account.Core.Events;

public class CustomerRegisteredEvent : IDomainEvent
{
    public Customer Customer { get; }
    
    public CustomerRegisteredEvent(Customer customer)
    {
        Customer = customer;
    }
}