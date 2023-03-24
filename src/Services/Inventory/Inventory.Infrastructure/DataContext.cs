using Ecommerce.Common.Core;
using Inventory.Core.Product;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure;

public class DataContext : DbContext
{
    public DbSet<Product> Products { get; set; }

    private IMediator _mediator;
    
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
            entity.OwnsMany(p => p.Information);
        });
    }

    public override int SaveChanges()
    {
        // first save the changes to the repo, then publish so the published data is always persisted correctly
        var result=  base.SaveChanges();
        
        var domainEntities = this.ChangeTracker
            .Entries<EntityRoot>()
            .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any());

        var domainEvents = domainEntities
            .SelectMany(x => x.Entity.DomainEvents)
            .ToList();

        foreach (var domainEvent in domainEvents)
            _mediator.Publish(domainEvent);

        return result;
    }
}