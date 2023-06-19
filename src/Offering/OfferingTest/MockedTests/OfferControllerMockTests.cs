﻿using Microsoft.AspNetCore.Mvc;
using Moq;
using Offering.Controllers;
using Offering.Dtos;
using Offering.Models;
using Offering.Repositories;

namespace OfferingTest;

public class OfferControllerMockTests
{
    [Fact]
    public async void ListOffers_Should_Return_All_Offers()
    {
        // Arrange
        var repositoryMock = new Mock<IOfferRepository>();
        repositoryMock
            .Setup(r => r.FindAll(0, 25))
            .ReturnsAsync(
                new List<Offer>
                {
                    SingleOffer.Create("Offer 1", Price.CreateFromGross(10, 19), DateTime.Now,
                        DateTime.Now.AddDays(1), Product.Create(Guid.NewGuid(), "Product 1", "Description"), Localization.Create("DE", "Germany", "Deutschland", "EUR"))
                });
        
        var productRepositoryMock = new Mock<IProductRepository>();
        var localizationRepositoryMock = new Mock<ILocalizationRepository>();

        var controller = new OfferController(repositoryMock.Object, productRepositoryMock.Object, localizationRepositoryMock.Object);

        // Act
        var result = await controller.ListOffers();

        // Assert
        repositoryMock.Verify(r => r.FindAll(0, 25), Times.Once);
        var okResult = result as OkObjectResult;
        var value = okResult.Value as List<Offer>;
        Assert.Equal(1, value.Count);
    }


    [Fact]
    public async void CreateSingleOffer_Should_Be_Persisted()
    {
        // Arrange
        var repositoryMock = new Mock<IOfferRepository>();
        var productRepositoryMock = new Mock<IProductRepository>();

        
        var product = Product.Create(Guid.NewGuid(), "Product 2", "Description");
        var createSingleOfferRequestDto = new CreateSingleOfferRequestDto("Offer 2", 10, 19, DateTime.Now, DateTime.Now.AddDays(1), product.Id);

        productRepositoryMock.Setup(r => r.FindById(product.Id)).ReturnsAsync(product);
        repositoryMock.Setup(r => r.Add(It.IsAny<SingleOffer>())).ReturnsAsync(SingleOffer.Create(createSingleOfferRequestDto.name, Price.CreateFromGross(createSingleOfferRequestDto.grossPrice, createSingleOfferRequestDto.taxValue), createSingleOfferRequestDto.startDate, createSingleOfferRequestDto.endDate, product, Localization.Create("DE", "Germany", "Deutschland", "EUR")));
        var localizationRepositoryMock = new Mock<ILocalizationRepository>();

        var controller = new OfferController(repositoryMock.Object, productRepositoryMock.Object, localizationRepositoryMock.Object);
        
        // Act
        var result = await controller.CreateSingleOffer(createSingleOfferRequestDto);
        
        // Assert
        repositoryMock.Verify(r => r.Add(It.IsAny<SingleOffer>()), Times.Once);
        var okResult = result as OkObjectResult;
        var value = okResult.Value as SingleOffer;
        Assert.Equal("Offer 2", value.Name);
    }
}