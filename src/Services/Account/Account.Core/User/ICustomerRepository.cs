namespace Account.Core.User;

public interface ICustomerRepository
{
    void Create(Customer customer);
    Customer GetByEmail(string email);
    void Update(Customer customer);
    void Delete(Guid id);
}