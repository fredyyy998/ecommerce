
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Core.Product;

namespace ShoppingCart.Infrastructure;

public class DataContext : DbContext
{
    public DbSet<Product> Products { get; set; }
    
    public DbSet<Core.ShoppingCart.ShoppingCart> ShoppingCarts { get; set; }
    
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
        });
        
        modelBuilder.Entity<Core.ShoppingCart.ShoppingCart>(entity =>
        {
            entity.OwnsMany(p => p.Items);
        });
    }
}