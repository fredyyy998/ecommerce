using Account.Core.Common;
using Account.Core.User;
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
        foreach (var domainEvent in entity.DomainEvents)
        {
            _mediator.Publish(domainEvent);
        }
    }
}