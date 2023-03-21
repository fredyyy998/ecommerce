using Inventory.Core.Product;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure;

public class DataContext : DbContext
{
    public DbSet<Product> Products { get; set; }
    
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        // necessary to save DateTime
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(entity =>
        {
            entity.OwnsOne(p => p.Price);
            entity.OwnsMany(p => p.Information);
        });
    }
    
}