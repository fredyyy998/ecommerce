namespace Account.Core.User;

public interface ICustomerRepository
{
    void Create(Customer customer);
    Customer GetByEmail(string email);
    bool EmailExists(string email);
    Customer GetById(Guid id);
    void Update(Customer customer);
    void Delete(Guid id);
}