using Account.Core.Common;
using Account.Core.User;

namespace Account.Core.Events;

public class CustomerEditedEvent : IDomainEvent
{
    public Customer Customer { get; }
    
    public CustomerEditedEvent(Customer customer)
    {
        Customer = customer;
    }
}