using Offering.Models;
using Offering.Repositories;

namespace OfferingTest;

public class OfferRepositoryTest : IClassFixture<TestDatabaseFixture>, IDisposable
{
    
    public TestDatabaseFixture Fixture { get; }

    public OfferRepositoryTest(TestDatabaseFixture fixture)
    {
        Fixture = fixture;
    }
    
    [Theory()]
    [InlineData(0, 6)]
    [InlineData(1, 5)]
    [InlineData(2, 4)]
    [InlineData(6, 0)]
    public async void Find_All_Offers(int skip, int resultCount)
    {
        // Arrange
        Fixture.CreateMockDbData();
        var repository = CreateOfferRepository(Fixture.CreateContext());
        
        // Act
        var result = await repository.FindAll(1, 10);
        
        // Assert
        Assert.Equal(5, result.Count);
    }
    
    [Fact]
    public async void Find_All_Available_Offers()
    {
        // Arrange
        Fixture.CreateMockDbData();
        var repository = CreateOfferRepository(Fixture.CreateContext());
        
        // Act
        var result = await repository.FindAllAvailable();
        
        // Assert
        Assert.Equal(4, result.Count);
    }

    [Fact]
    public async void Find_All_Expired_Offers()
    {
        // Arrange
        Fixture.CreateMockDbData();
        var repository = CreateOfferRepository(Fixture.CreateContext());
        
        // Act
        var result = await repository.FindAllExpired();
        
        // Assert
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async void Find_By_Id_Returns_Correct_When_Exists()
    {
        // Arrange
        Fixture.CreateMockDbData();
        var repository = CreateOfferRepository(Fixture.CreateContext());
        
        // Act
        var result = await repository.FindById(Guid.Parse("78e13d5e-da89-4d09-8249-dcef0858f924"));
        
        // Assert
        Assert.Equal(Guid.Parse("78e13d5e-da89-4d09-8249-dcef0858f924"), result.Id);
        Assert.Equal("Offer 1", result.Name);
    }
    
    [Fact]
    public async void Find_By_Id_Returns_Null_When_Not_Exists()
    {
        // Arrange
        Fixture.CreateMockDbData();
        var repository = CreateOfferRepository(Fixture.CreateContext());
        
        // Act
        var result = await repository.FindById(Guid.Parse("78e13d5e-da89-4d09-8249-dcef0858f925"));
        
        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async void Finds_Offers_By_Product()
    {
        // Arrange
        Fixture.CreateMockDbData();
        var repository = CreateOfferRepository(Fixture.CreateContext());
        
        // Act
        var result = await repository.findByProduct(Guid.Parse("43ff93b2-3272-4dbf-bcd3-263212e7a7f6"));
        
        // Assert
        Assert.Equal(3, result.Count);
    }

    [Fact]
    public async void Persists_New_Offer_And_Returns_It()
    {
        // Arrange
        var context = Fixture.CreateContext();
        Fixture.CreateMockDbData();
        var price = Price.CreateFromGross(10, 19);
        var localisation = context.Localizations.First();
        var product = context.Products.First();
        var offer = SingleOffer.Create( "Offer New", price, DateTime.Now.AddDays(-10), DateTime.Now.AddDays(10), product, localisation);
        var repository = CreateOfferRepository(context);
        
        // Act
        var result = await repository.Add(offer);
        
        // Assert
        Assert.NotNull(offer.Id);
        Assert.Equal(offer.Name, result.Name);
        Assert.Equal(7, context.Offers.Count());
    }
    
    [Fact]
    public async void Persists_New_Offer_With_new_Product()
    {
        // Arrange
        var context = Fixture.CreateContext();
        Fixture.CreateMockDbData();
        var price = Price.CreateFromGross(10, 19);
        var localisation = context.Localizations.First();
        var product = Product.Create(Guid.Parse("f1273588-40fd-4050-b956-a61ae82db4d1"), "Prod name", "description");
        var offer = SingleOffer.Create( "Offer New", price, DateTime.Now.AddDays(-10), DateTime.Now.AddDays(10), product, localisation);
        var repository = CreateOfferRepository(context);
        
        // Act
        var result = await repository.Add(offer);
        
        // Assert
        var persistedProduct = await context.Products.FindAsync(Guid.Parse("f1273588-40fd-4050-b956-a61ae82db4d1"));
        Assert.Equal(product.Name, persistedProduct.Name);
        Assert.Equal(product.Id, persistedProduct.Id);
        
        Assert.Equal(7, context.Offers.Count());
    }

    [Fact]
    public async void Persists_Update()
    {
        // Arrange
        var context = Fixture.CreateContext();
        var repository = CreateOfferRepository(context);
        Fixture.CreateMockDbData();
        var offer = await context.Offers.FindAsync(Guid.Parse("78e13d5e-da89-4d09-8249-dcef0858f924"));
        

        // Act
        var discount = Discount.Create(10, DateTime.Now, DateTime.Now.AddDays(10));
        offer.ApplyDiscount(discount);
        await repository.Update(offer);
        
        // Assert
        var persistedOffer = await context.Offers.FindAsync(Guid.Parse("78e13d5e-da89-4d09-8249-dcef0858f924"));
        Assert.Equal(10, persistedOffer.Discount.DiscountRate);
    }

    [Fact]
    public async void Removes_Offer()
    {
        // Arrange
        var context = Fixture.CreateContext();
        var repository = CreateOfferRepository(context);
        Fixture.CreateMockDbData();
        var offer = await context.Offers.FindAsync(Guid.Parse("78e13d5e-da89-4d09-8249-dcef0858f924"));
        
        // Act
        await repository.Delete(offer);
        
        // Assert
        Assert.Equal(5, context.Offers.Count());
    }

    public void Dispose()
    {
        Fixture.Cleanup();
    }
    
    private IOfferRepository CreateOfferRepository(DataContext context)
    {
        return new OfferRepository(context);
    }
}