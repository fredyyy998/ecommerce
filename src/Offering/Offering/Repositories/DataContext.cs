using Microsoft.EntityFrameworkCore;
using Offering.Models;

namespace Offering.Repositories;

public class DataContext : DbContext
{
    public DbSet<Product> Products { get; set; }
    
    public DbSet<Offer> Offers { get; set; }
    
    public DbSet<Localization> Localizations { get; set; }

    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Offer>()
            .HasDiscriminator<string>("OfferType")
            .HasValue<SingleOffer>("SingleOffer")
            .HasValue<PackageOffer>("PackageOffer");

        modelBuilder.Entity<Offer>()
            .OwnsOne(o => o.Price);
        
        modelBuilder.Entity<Offer>()
            .OwnsOne(o => o.Discount);

        modelBuilder.Entity<Offer>()
            .HasOne(o => o.Localization)
            .WithMany()
            .HasForeignKey("LocalizationCountryCode")
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<SingleOffer>()
            .HasOne(so => so.Product)
            .WithMany()
            .HasForeignKey("ProductId")
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<PackageOffer>()
            .HasMany(po => po.Products)
            .WithMany()
            .UsingEntity(j => j.ToTable("PackageOfferProducts"));
        
        modelBuilder.Entity<Localization>()
            .HasKey(l => l.CountryCode);
    }
}