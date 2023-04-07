using Account.Core.User;
using Account.Infrastructure;
using Account.Infrastructure.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Account.Test.User;

public class CustomerRepositoryTest
{
    private readonly DataContext _dbContext;
    private readonly ICustomerRepository _customerRepository;
    private readonly Mock<IMediator> _mediatorMock;

    public CustomerRepositoryTest()
    {
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: "testDb")
            .Options;
        
        _mediatorMock = new Mock<IMediator>();

        _dbContext = new DataContext(options, _mediatorMock.Object);
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

    [Fact]
    public void Create_Customer_published_events_to_mediator()
    {
        var customer = Customer.Create("test@email.de", "abc1234");
        
        _customerRepository.Create(customer);

        _mediatorMock.Verify(m => m.Publish(It.IsAny<INotification>(), It.IsAny<CancellationToken>()), Times.Once);
    }
    
    [Fact]
    public void Create_Customer_published_events_to_on_update()
    {
        var customer = Customer.Create("test@email.de", "abc1234");
        _customerRepository.Create(customer);

        _customerRepository.Update(customer);

        _mediatorMock.Verify(m => m.Publish(It.IsAny<INotification>(), It.IsAny<CancellationToken>()), Times.Exactly(1));
    }
    
    [Fact]
    public void Create_Customer_published_events_to_on_delete()
    {
        var customer = Customer.Create("test@email.de", "abc1234");
        _customerRepository.Create(customer);

        _customerRepository.Delete(customer.Id);

        _mediatorMock.Verify(m => m.Publish(It.IsAny<INotification>(), It.IsAny<CancellationToken>()), Times.Exactly(1));
    }
}