using Account.Core.Administrator;
using Account.Core.Events;
using Account.Core.User;
using Ecommerce.Common.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Account.Infrastructure;

public class DataContext : DbContext
{
    public DbSet<Customer> Customers { get; set; }
    
    public DbSet<Administrator> Administrator { get; set; }

    private readonly IMediator _mediator;

    public DataContext(DbContextOptions<DataContext> options, IMediator mediator) : base(options)
    {
        _mediator = mediator;
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
        
        modelBuilder.Entity<Administrator>(entity => entity.OwnsOne(c => c.Password));
        
        // Seed the database
        
        var admin = Core.Administrator.Administrator.Create("ecommerce@admin.de", "!abc123?");
        
        modelBuilder.Entity<Administrator>(entity => entity.OwnsOne(c => c.Password)
            .HasData(new { AdministratorId =  admin.Id, Hash = admin.Password.Hash, Salt = admin.Password.Salt }));

        modelBuilder.Entity<Administrator>().HasData(
            new { Id = admin.Id, Email = admin.Email, Role = "Admin", CreatedAt = admin.CreatedAt, UpdatedAt = admin.UpdatedAt }
        );
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        var result = await base.SaveChangesAsync(cancellationToken);
        
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
    
    private IReadOnlyCollection<IDomainEvent> MergeDomainEvents(IReadOnlyCollection<IDomainEvent> domainEvents)
    {
        var publishEvents = domainEvents.ToList();
        var eventsToDelete = publishEvents.Where(x => x.GetType() == typeof(CustomerEditedEvent)).Reverse().Skip(1);
        
        foreach (var obj in eventsToDelete.ToList())
        {
            publishEvents.Remove(obj);
        }

        return publishEvents;
    }
}