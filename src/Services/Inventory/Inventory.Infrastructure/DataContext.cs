using Ecommerce.Common.Core;
using Inventory.Core.DomainEvents;
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
            entity.OwnsMany(p => p.Informations);
        });
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        var result=  await base.SaveChangesAsync(cancellationToken);
        
        var domainEntities = this.ChangeTracker
            .Entries<EntityRoot>()
            .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any());

        domainEntities
            .SelectMany(x => MergeDomainEvents(x.Entity))
            .ToList();
        
        var domainEvents = domainEntities
            .SelectMany(x => x.Entity.DomainEvents)
            .ToList();

        foreach (var domainEvent in domainEvents)
            _mediator.Publish(domainEvent);

        return result;
    }

    private List<IDomainEvent> MergeDomainEvents(EntityRoot entityRoot)
    {
        var domainEvents = entityRoot.DomainEvents.ToList();
        domainEvents = MergeProductUpdatedByAdminEvent(domainEvents);

        return domainEvents;
    }

    // only the last updated event is relevant since that's the current product, there is no need to publish the previous ones
    private List<IDomainEvent> MergeProductUpdatedByAdminEvent(List<IDomainEvent> domainEvents)
    {
        var productUpdatedEvents = domainEvents
            .Where(e => e.GetType() == typeof(ProductUpdatedByAdminEvent))
            .Take(domainEvents.Count - 1)
            .ToList();
        domainEvents.RemoveAll(e => productUpdatedEvents.Contains(e));
        return domainEvents;
    }
}