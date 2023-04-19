using Account.Core.User;
using Ecommerce.Common.Core;
using Microsoft.EntityFrameworkCore;

namespace Account.Infrastructure.Repository;

public class CustomerRepository : ICustomerRepository
{
    private readonly DataContext _dbContext;

    public CustomerRepository(DataContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PagedList<Customer>> FindAll(PaginationParameter paginationParameter)
    {
        var query = _dbContext.Customers.AsQueryable();
        return PagedList<Customer>.ToPagedList(query, paginationParameter.PageNumber, paginationParameter.PageSize);
    }

    public async Task<Customer> Create(Customer customer)
    {
        _dbContext.Customers.Add(customer);
        await _dbContext.SaveChangesAsync();
        return await GetById(customer.Id);
    }

    public Task<Customer> GetByEmail(string email)
    {
        return _dbContext.Customers.FirstOrDefaultAsync(c => c.Email == email);
    }

    public async Task<bool> EmailExists(string email)
    {
        return (await GetByEmail(email)) != null;
    }

    public Task<Customer> GetById(Guid id)
    {
        return _dbContext.Customers.FirstOrDefaultAsync(c => c.Id == id);
    }

    public Task Update(Customer customer)
    {
        _dbContext.Customers.Update(customer);
        return _dbContext.SaveChangesAsync();
    }

    public async Task Delete(Guid id)
    {
        var customer = await GetById(id);
        _dbContext.Customers.Remove(customer);
        _dbContext.SaveChangesAsync();
    }
}