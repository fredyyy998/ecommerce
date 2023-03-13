using Account.Core.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Account.Infrastructure;

public class DataContext : DbContext
{
    public DbSet<Customer> Customers { get; set; }


    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        // necessary to save DateTime
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.OwnsOne(c => c.Address, address =>
            {
                address.Property(a => a.Street).IsRequired(false);
                address.Property(a => a.City).IsRequired(false);
                address.Property(a => a.Zip).IsRequired(false);
                address.Property(a => a.Country).IsRequired(false);
            });

            entity.OwnsOne(c => c.PaymentInformation, pi =>
            {
                pi.OwnsOne(p => p.Address, address =>
                {
                    address.Property(a => a.Street).IsRequired(false);
                    address.Property(a => a.City).IsRequired(false);
                    address.Property(a => a.Zip).IsRequired(false);
                    address.Property(a => a.Country).IsRequired(false);
                });
            });

            entity.OwnsOne(c => c.PersonalInformation, pi =>
            {
                pi.Property(p => p.FirstName).IsRequired(false);
                pi.Property(p => p.LastName).IsRequired(false);
                // TODO the DateOnly type cannot be set to nullable, tho for now we simply set an unrealistic default value
                // TODO this is a workaround, we should find a better solution
                pi.Property(p => p.DateOfBirth).HasDefaultValue(DateOnly.Parse("1900-01-01"));
            });
            entity.OwnsOne(c => c.Password);
        });
    }
}