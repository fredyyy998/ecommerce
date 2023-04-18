using Ecommerce.Common.Core;
using Fulfillment.Core.Buyer;
using Fulfillment.Core.Order;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fulfillment.Infrastructure;

public class DataContext : DbContext
{
    public DbSet<Buyer> Buyers { get; set; }
    
    public DbSet<Order> Orders { get; set; }

    private readonly IMediator _mediator;
    
    public DataContext(DbContextOptions<DataContext> options, IMediator mediator) : base(options)
    {
        _mediator = mediator;
        // necessary to save DateTime
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Buyer>(entity =>
        {

            entity.OwnsOne(c => c.PersonalInformation, pi =>
            {
                pi.Property(p => p.FirstName).IsRequired(false);
                pi.Property(p => p.LastName).IsRequired(false);
                pi.Property(p => p.Email);
            });
            entity.OwnsOne(c => c.ShippingAddress, address =>
            {
                address.Property(a => a.Street).IsRequired(false);
                address.Property(a => a.City).IsRequired(false);
                address.Property(a => a.ZipCode).IsRequired(false);
                address.Property(a => a.Country).IsRequired(false);
            });
            
            entity.OwnsOne(c => c.PaymentInformation, pi =>
            {
                pi.OwnsOne(p => p.Address, address =>
                {
                    address.Property(a => a.Street).IsRequired(false);
                    address.Property(a => a.City).IsRequired(false);
                    address.Property(a => a.ZipCode).IsRequired(false);
                    address.Property(a => a.Country).IsRequired(false);
                });
            });
        });
        
        modelBuilder.Entity<Order>(entity =>
        {
            entity.OwnsOne(o => o.TotalPrice);
            entity.OwnsMany(o => o.Products, p =>
            {
                p.WithOwner().HasForeignKey("OrderId");
                p.Property<int>("Id").UseIdentityColumn();
                p.HasKey("Id");
                p.OwnsOne(i => i.Price);
                p.OwnsOne(i => i.TotalPrice);
            });
            entity.OwnsOne(o => o.ShipmentAddress, address =>
            {
                address.Property(a => a.Street);
                address.Property(a => a.City);
                address.Property(a => a.ZipCode);
                address.Property(a => a.Country);
            });
        });
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        var result = base.SaveChangesAsync(cancellationToken);

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