using Ecommerce.Common.Core;

namespace Account.Core.User;

public interface ICustomerRepository : IRepository<Customer>
{
    Customer GetByEmail(string email);
    bool EmailExists(string email);
}