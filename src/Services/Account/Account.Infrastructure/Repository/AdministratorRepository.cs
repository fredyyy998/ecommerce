using Account.Core.Administrator;

namespace Account.Infrastructure.Repository;

public class AdministratorRepository : IAdministratorRepository
{
    private readonly DataContext _dbContext;

    public AdministratorRepository(DataContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Administrator GetAdministrator(string email)
    {
        return _dbContext.Administrators.FirstOrDefault(a => a.Email == email);
    }

    public bool EmailExists(string email)
    {
        return GetAdministrator(email) != null;
    }
}