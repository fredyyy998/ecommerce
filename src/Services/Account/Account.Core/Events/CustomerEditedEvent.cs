using Account.Core.User;
using Ecommerce.Common.Core;

namespace Account.Core.Events;

public class CustomerEditedEvent : IDomainEvent
{
    public Customer Customer { get; }
    
    public CustomerEditedEvent(Customer customer)
    {
        Customer = customer;
    }
}