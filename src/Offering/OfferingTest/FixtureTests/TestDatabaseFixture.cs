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
        context.SaveChanges();
        
        CreateMockProducts(context);
        CreateMockOffers(context);
        
        
    }

    private void CreateMockProducts(DataContext context)
    {
        var product1 = Product.Create(Guid.Parse("e2bf10b3-ea7f-47a4-a70e-160fd1b4abe5"), "Product 1", "Description");
        var product2 = Product.Create(Guid.Parse("c2511115-cb17-4deb-8c00-d5b1b787327a"), "Product 2", "Description");
        var product3 = Product.Create(Guid.Parse("78fb0cae-4b1b-417b-91c3-f483fce69761"), "Product 3", "Description");
        var product4 = Product.Create(Guid.Parse("07d6faf7-ad39-4810-82f7-1f4b40e3c877"), "Product 4", "Description");
        var product5 = Product.Create(Guid.Parse("43ff93b2-3272-4dbf-bcd3-263212e7a7f6"), "Product 5", "Description");
        context.Products.AddRange(product1, product2, product3, product4, product5);
        context.SaveChanges();
    }

    public void CreateMockOffers(DataContext context)
    {
        var localisation = context.Localizations.First();
        var price = Price.CreateFromGross(10, 19);
        var offer = SingleOffer.Create(Guid.Parse("78e13d5e-da89-4d09-8249-dcef0858f924"), "Offer 1", price, DateTime.Now, DateTime.Now.AddDays(1), GetProductById(Guid.Parse("43ff93b2-3272-4dbf-bcd3-263212e7a7f6"), context), localisation);
        var price2 = Price.CreateFromGross(10, 19);
        var offer2 = SingleOffer.Create(Guid.Parse("7d67521c-6a66-428d-918d-d77a7780892d"),"Offer 2", price2, DateTime.Now.AddDays(-10), DateTime.Now.AddDays(-5),  GetProductById(Guid.Parse("07d6faf7-ad39-4810-82f7-1f4b40e3c877"), context), localisation);
        var price3 = Price.CreateFromGross(10, 19);
        var offer3 = SingleOffer.Create(Guid.Parse("c216d6f5-c2cf-4ada-b816-13a23c0cce70"), "Offer 3", price3, DateTime.Now.AddDays(-10), DateTime.Now.AddDays(-5), GetProductById(Guid.Parse("c2511115-cb17-4deb-8c00-d5b1b787327a"), context), localisation);
        var price4 = Price.CreateFromGross(10, 19);
        var offer4 = SingleOffer.Create(Guid.Parse("e36f7dc3-ea03-41e8-bbb1-508e970f43bd"), "Offer 4", price4, DateTime.Now.AddDays(-10), DateTime.Now.AddDays(10), GetProductById(Guid.Parse("07d6faf7-ad39-4810-82f7-1f4b40e3c877"), context), localisation);
        
        var price5 = Price.CreateFromGross(10, 19);
        var offer5 = PackageOffer.Create(Guid.Parse("ac0b52d3-54cd-4572-b32d-6f65aed7e610"),"Package Offer 1", price5, DateTime.Now.AddDays(-10), DateTime.Now.AddDays(15), new List<Product> { GetProductById(Guid.Parse("07d6faf7-ad39-4810-82f7-1f4b40e3c877"), context), GetProductById(Guid.Parse("43ff93b2-3272-4dbf-bcd3-263212e7a7f6"), context) }, localisation);
        var price6 = Price.CreateFromGross(10, 19);
        var offer6 = PackageOffer.Create(Guid.Parse("d40c43d3-7285-4fab-aea4-9343e99e6bb6"),"Package Offer 2", price6, DateTime.Now.AddDays(-10), DateTime.Now.AddDays(2), new List<Product> { GetProductById(Guid.Parse("c2511115-cb17-4deb-8c00-d5b1b787327a"), context), GetProductById(Guid.Parse("43ff93b2-3272-4dbf-bcd3-263212e7a7f6"), context) }, localisation);
        
        context.Offers.AddRange(offer, offer2, offer3,offer4, offer5, offer6);
        context.SaveChanges();
    }
    
    public Product GetProductById(Guid id, DataContext context)
    {
        return context.Products.FirstOrDefault(p => p.Id == id);
    }
    
    public DataContext CreateContext()
        => new DataContext(
            new DbContextOptionsBuilder<DataContext>()
                .UseMySQL(ConnectionString)
                .Options);
}