using Account.Core.User;

namespace Account.Infrastructure.Repository;

public class CustomerRepository : ICustomerRepository
{
    private readonly DataContext _dbContext;
    
    public CustomerRepository(DataContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public void Create(Customer customer)
    {
        _dbContext.Customers.Add(customer);
        _dbContext.SaveChanges();
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
    }

    public void Delete(Guid id)
    {
        var customer = GetById(id);
        _dbContext.Customers.Remove(customer);
        _dbContext.SaveChanges();
    }
}