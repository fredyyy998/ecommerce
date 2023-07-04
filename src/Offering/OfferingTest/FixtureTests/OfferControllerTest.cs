using Microsoft.AspNetCore.Mvc;
using Offering.Controllers;
using Offering.Dtos;
using Offering.Models;
using Offering.Repositories;

namespace OfferingTest;

public class OfferControllerTest : IClassFixture<TestDatabaseFixture>, IDisposable
{
    public TestDatabaseFixture Fixture { get; }

    public OfferControllerTest(TestDatabaseFixture fixture)
    {
        Fixture = fixture;
    }

    public void Dispose()
    {
        Fixture.Cleanup();
    }

    [Fact]
    public async void ListOffers_Should_Return_All_Offers()
    {
        // Arrange
        Fixture.CreateMockDbData();
        var context = Fixture.CreateContext();
        var _offerRepository = new OfferRepository(context);
        var _productRepository = new ProductRepository(context);
        var _localisationRepository = new LocalizationRepository(context);
        var controller = new OfferController(_offerRepository, _productRepository, _localisationRepository);
        
        // Act
        var result = await controller.ListOffers();
        
        // Assert
        var okResult = result as OkObjectResult;
        var value = okResult.Value as List<Offer>;
        Assert.Equal(1, value.Count);
    }

    [Fact]
    public async void CreateSingleOffer_Should_Be_Persisted()
    {
        // Arrange
        var context = Fixture.CreateContext();
        var _offerRepository = new OfferRepository(context);
        var _productRepository = new ProductRepository(context);
        var _localisationRepository = new LocalizationRepository(context);
        context.Localizations.Add(Localization.Create("DE", "Germany", "Deutschland", "EUR"));
        context.SaveChanges();
        var controller = new OfferController(_offerRepository, _productRepository, _localisationRepository);
        var product = Product.Create(Guid.NewGuid(), "Product 2", "Description");
        await _productRepository.Add(product);
        var createSingleOfferRequestDto = new CreateSingleOfferRequestDto("Offer 2", 10, 19, DateTime.Now, DateTime.Now.AddDays(1), product.Id);

        // Act
        var result = await controller.CreateSingleOffer(createSingleOfferRequestDto);
        
        // Assert
        var okResult = result as OkObjectResult;
        var value = okResult.Value as SingleOffer;
        Assert.Equal("Offer 2", value.Name);
        var offer = context.Offers.Single(o => o.Name == "Offer 2");
        Assert.Equal(offer.Name, value.Name);
    }
    
}