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

        Cleanup();
    }

    public void Cleanup()
    {
        var context = CreateContext();
        
        context.RemoveRange(context.Offers);
        context.RemoveRange(context.Products);
        context.RemoveRange(context.Localizations);
        context.SaveChanges();
        
    }

    public void CreateMockDbData()
    {
        var context = CreateContext();
        
        var localisation = Localization.Create("DE", "Germany", "Deutschland", "EUR");
        context.Localizations.Add(localisation);

        var price = Price.CreateFromGross(10, 19);
        var product1 = Product.Create(Guid.NewGuid(), "Product 1", "Description");
        var offer = SingleOffer.Create("Offer 1", price, DateTime.Now, DateTime.Now.AddDays(1), product1, localisation);
        context.Offers.AddRange(offer);
        context.SaveChanges();
    }
    
    public DataContext CreateContext()
        => new DataContext(
            new DbContextOptionsBuilder<DataContext>()
                .UseMySQL(ConnectionString)
                .Options);
}