namespace Account.Core.Administrator;

public interface IAdministratorRepository
{
    Task<Administrator> GetAdministrator(string email);
    Task<bool> EmailExists(string email);
}