using Microsoft.EntityFrameworkCore;
using Offering.Models;
using Offering.Repositories;

namespace OfferingTest;

public class TestDatabaseFixture
{
    private const string ConnectionString = "server=localhost;database=db;user=offer-user;password=password";

    public TestDatabaseFixture()
    {
        using var context = CreateContext();
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        Cleanup();
    }

    public void Cleanup()
    {
        var context = CreateContext();
        
        context.RemoveRange(context.Offers);
        context.RemoveRange(context.Products);

        var price = Price.CreateFromGross(10, 19, "EUR");
        var product1 = Product.Create(Guid.NewGuid(), "Product 1", "Description");
        var product2 = Product.Create(Guid.NewGuid(), "Product 2", "Description");
        var offer = SingleOffer.Create("Offer 1", price, DateTime.Now, DateTime.Now.AddDays(1), product1);
        context.AddRange(
            product1,
            product2,
            offer
        );
        context.SaveChanges();
        
    }
    
    public DataContext CreateContext()
        => new DataContext(
            new DbContextOptionsBuilder<DataContext>()
                .UseMySQL(ConnectionString)
                .Options);
}