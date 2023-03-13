using Account.Core.User;
using Account.Infrastructure;
using Account.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;

namespace Account.Test.User;

public class CustomerRepositoryTest
{
    private readonly DataContext _dbContext;
    private readonly ICustomerRepository _customerRepository;

    public CustomerRepositoryTest()
    {
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: "testDb")
            .Options;

        _dbContext = new DataContext(options);
        _customerRepository = new CustomerRepository(_dbContext);
    }

    [Fact]
    public void Create_Customer_Should_Be_Added_To_Database()
    {
        var customer = Customer.Create("test@email.de", "abc1234");
        
        _customerRepository.Create(customer);
        
        Assert.Equal(1, _dbContext.Customers.Count());
        Assert.Contains(customer, _dbContext.Customers);
    }
}