using Account.Core.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Account.Infrastructure;

public class DataContext : DbContext
{
    public DbSet<Customer> Customers { get; set; }
    
    protected readonly IConfiguration Configuration;
    
    
    public DataContext(IConfiguration configuration)
    {
        Configuration = configuration;
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase(databaseName: "AccountDatabase");
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.OwnsOne(c => c.Address);
            entity.OwnsOne(c => c.PaymentInformation, pi =>
                {
                    pi.OwnsOne(c => c.Address);
                });
            entity.OwnsOne(c => c.PersonalInformation);
            entity.OwnsOne(c => c.Password);
        });
    }
}