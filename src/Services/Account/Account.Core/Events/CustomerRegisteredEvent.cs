using Account.Core.Common;
using Account.Core.User;
using MediatR;

namespace Account.Core.Events;

public class CustomerRegisteredEvent : IDomainEvent
{
    public Customer Customer { get; }
    
    public CustomerRegisteredEvent(Customer customer)
    {
        Customer = customer;
    }
}