using Fulfillment.Core.Buyer;
using Fulfillment.Core.Order;
using Microsoft.EntityFrameworkCore;

namespace Fulfillment.Infrastructure;

public class DataContext : DbContext
{
    public DbSet<Buyer> Buyers { get; set; }
    
    public DbSet<Order> Orders { get; set; }
    
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        // necessary to save DateTime
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Buyer>(entity =>
        {
            entity.OwnsOne(b => b.PersonalInformation);
            entity.OwnsOne(b => b.ShippingAddress);
            entity.OwnsOne(b => b.PaymentInformation);
        });
        
        modelBuilder.Entity<Order>(entity =>
        {
            entity.OwnsOne(o => o.TotalPrice);
            entity.OwnsMany(o => o.Products, p =>
            {
                p.WithOwner().HasForeignKey("OrderId");
                p.Property<int>("Id").UseIdentityColumn();
                p.HasKey("Id");
            });
        });
    }
}