namespace Account.Core.Administrator;

public interface IAdministratorRepository
{
    Administrator GetAdministrator(string email);
    bool EmailExists(string email);
}