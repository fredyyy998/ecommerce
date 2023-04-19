using Account.Core.Administrator;
using Microsoft.EntityFrameworkCore;

namespace Account.Infrastructure.Repository;

public class AdministratorRepository : IAdministratorRepository
{
    private readonly DataContext _dbContext;

    public AdministratorRepository(DataContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<Administrator> GetAdministrator(string email)
    {
        return _dbContext.Administrator.FirstOrDefaultAsync(a => a.Email == email);
    }

    public async Task<bool> EmailExists(string email)
    {
        return (await GetAdministrator(email)) != null;
    }
}