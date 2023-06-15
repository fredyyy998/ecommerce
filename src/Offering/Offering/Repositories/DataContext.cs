using Microsoft.EntityFrameworkCore;
using Offering.Models;

namespace Offering.Repositories;

public class DataContext : DbContext
{
    public DbSet<Product> Products { get; set; }
    
    public DbSet<Offer> Offers { get; set; }

    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Offer>()
            .HasDiscriminator<string>("OfferType")
            .HasValue<SingleOffer>("SingleOffer")
            .HasValue<PackageOffer>("PackageOffer");
    }
}