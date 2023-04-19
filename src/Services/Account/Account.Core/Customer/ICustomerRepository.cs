using Ecommerce.Common.Core;

namespace Account.Core.User;

public interface ICustomerRepository : IRepository<Customer>
{
    Task<Customer> GetByEmail(string email);
    Task<bool> EmailExists(string email);
}