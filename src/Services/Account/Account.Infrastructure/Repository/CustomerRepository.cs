using Account.Core.Events;
using Account.Core.User;
using Ecommerce.Common.Core;
using MediatR;

namespace Account.Infrastructure.Repository;

public class CustomerRepository : ICustomerRepository
{
    private readonly DataContext _dbContext;
    
    private readonly IMediator _mediator;
    
    public CustomerRepository(DataContext dbContext, IMediator mediator)
    {
        _dbContext = dbContext;
        _mediator = mediator;
    }
    
    public void Create(Customer customer)
    {
        _dbContext.Customers.Add(customer);
        _dbContext.SaveChanges();
        
        PublishDomainEvents(customer);
    }

    public Customer GetByEmail(string email)
    {
        return _dbContext.Customers.FirstOrDefault(c => c.Email == email);
    }

    public bool EmailExists(string email)
    {
        return GetByEmail(email) != null;
    }

    public Customer GetById(Guid id)
    {
        return _dbContext.Customers.FirstOrDefault(c => c.Id == id);
    }

    public void Update(Customer customer)
    {
        _dbContext.Customers.Update(customer);
        _dbContext.SaveChanges();
        
        PublishDomainEvents(customer);
    }

    public void Delete(Guid id)
    {
        var customer = GetById(id);
        _dbContext.Customers.Remove(customer);
        _dbContext.SaveChanges();
        
        PublishDomainEvents(customer);
    }

    private void PublishDomainEvents(EntityRoot entity)
    {
        var publishEvents = MergeDomainEvents(entity.DomainEvents);
        foreach (var domainEvent in publishEvents)
        {
            _mediator.Publish(domainEvent);
        }
    }

    private IReadOnlyCollection<IDomainEvent> MergeDomainEvents(IReadOnlyCollection<IDomainEvent> domainEvents)
    {
        var publishEvents = domainEvents.ToList();
        var eventsToDelete = publishEvents.Where(x => x.GetType() == typeof(CustomerEditedEvent)).Reverse().Skip(1);
        
        foreach (var obj in eventsToDelete.ToList())
        {
            publishEvents.Remove(obj);
        }

        return publishEvents;
    }
}