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

        var product1 = Product.Create("MacBook", "13-inch, 8GB RAM, 256GB SSD, 2.3GHz Dual-Core Processor", 1299);
        product1.AddStock(10);
        SeedProduct(modelBuilder, product1);
        
        var product2 = Product.Create("iPhone 12 Pro", "6.1-inch Super Retina XDR display, 5G capable, 128GB storage", 999);
        product2.AddStock(25);
        SeedProduct(modelBuilder, product2);
        
        var product3 = Product.Create("Samsung Galaxy S21 Ultra", "6.8-inch Dynamic AMOLED 2X, 5G capable, 256GB storage", 1199);
        product3.AddStock(15);
        SeedProduct(modelBuilder, product3);
        
        var product4 = Product.Create("Sony PlayStation 5", "Gaming console with 4K UHD Blu-ray drive, 825GB SSD storage	", 499);
        product4.AddStock(20);
        SeedProduct(modelBuilder, product4);
        
        var product5 = Product.Create("LG OLED CX Series 65\" 4K Smart TV	", "65-inch OLED display, 4K UHD, webOS 5.0, Alexa and Google Assistant compatible	", 1999);
        product5.AddStock(5);
        SeedProduct(modelBuilder, product5);
    }

    private void SeedProduct(ModelBuilder modelBuilder, Product product)
    {
        modelBuilder.Entity<Product>().OwnsOne(p =>p.Price).HasData( new { ProductId = product.Id, GrossPrice = product.Price.GrossPrice, CurrencyCode = product.Price.CurrencyCode, NetPrice = product.Price.NetPrice, SalesTax = product.Price.SalesTax });
        modelBuilder.Entity<Product>().HasData(new { Id = product.Id, Name = product.Name, Description = product.Description, Stock = product.Stock });
    }

    public override int SaveChanges()
    {
        // first save the changes to the repo, then publish so the published data is always persisted correctly
        var result=  base.SaveChanges();
        
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