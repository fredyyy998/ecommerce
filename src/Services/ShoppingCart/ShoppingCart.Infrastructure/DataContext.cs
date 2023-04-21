
using Ecommerce.Common.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Core.Product;

namespace ShoppingCart.Infrastructure;

public class DataContext : DbContext
{
    public DbSet<Product> Products { get; set; }

    private readonly IMediator _mediator;
    
    public DbSet<Core.ShoppingCart.ShoppingCart> ShoppingCarts { get; set; }
    
    public DataContext(DbContextOptions<DataContext> options, IMediator mediator) : base(options)
    {
        _mediator = mediator;
        // necessary to save DateTime
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(entity =>
        {
            entity.OwnsOne(p => p.Price);
            
            entity.OwnsMany(p => p.Reservations, r =>
            {
                r.WithOwner().HasForeignKey("ProductId");
                r.Property<int>("Id").UseIdentityColumn();
                r.HasKey("Id");
            });
        });
        
        modelBuilder.Entity<Core.ShoppingCart.ShoppingCart>(entity =>
        {
            entity.OwnsMany(p => p.Items, i =>
            {
                i.WithOwner().HasForeignKey("ShoppingCartId");
                i.Property<int>("Id").UseIdentityColumn();
                i.HasKey("Id");
            });
            
            entity.OwnsOne(s => s.ShoppingCartCheckout, co =>
            {
                co.Property(c => c.CustomerId);
                co.Property(c => c.FirstName);
                co.Property(c => c.LastName);
                co.Property(c => c.Email);
                co.OwnsOne(c => c.ShippingAddress, sa =>
                {
                    sa.Property(a => a.Street);
                    sa.Property(a => a.City);
                    sa.Property(a => a.ZipCode);
                    sa.Property(a => a.Country);
                });
                co.OwnsOne(c => c.BillingAddress, ba =>
                {
                    ba.Property(a => a.Street);
                    ba.Property(a => a.City);
                    ba.Property(a => a.ZipCode);
                    ba.Property(a => a.Country);
                });
            });
        });
    }
    

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        var result = await base.SaveChangesAsync();

        var domainEntities = this.ChangeTracker
            .Entries<EntityRoot>()
            .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any());
        
        var domainEvents = domainEntities.SelectMany(x => x.Entity.DomainEvents).ToList();

        foreach (var domainEntity in domainEntities)
            domainEntity.Entity.ClearEvents();
        
        foreach (var domainEvent in domainEvents)
            _mediator.Publish(domainEvent);
        
        return result;
    }
}