using System.Collections.ObjectModel;
using AutoMapper;
using Inventory.Application.Dtos;
using Inventory.Application.Services;
using Inventory.Application.Utils;
using Inventory.Core.Product;
using Inventory.Core.Utility;
using MediatR;
using Moq;
using Xunit;

namespace Inventory.Test;

public class ProductServiceTest
{
    private readonly ProductService _productService;
    private readonly IMapper _mapper;
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly Mock<IMediator> _mockMediator;

    public ProductServiceTest()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        _mapper = config.CreateMapper();

        _productRepositoryMock = new Mock<IProductRepository>();
        _mockMediator = new Mock<IMediator>();
        _productService = new ProductService(_productRepositoryMock.Object, _mapper, _mockMediator.Object);
    }

    [Fact]
    public void Product_search_response_with_list()
    {
        // Arrange
        var product = Product.Create("test", "test", 1);
        var productParameters = new ProductParameters();
        productParameters.Search = "test";
        productParameters.PageNumber = 1;
        productParameters.PageSize = 20;
        _productRepositoryMock.Setup(x => x.GetAvailableProducts(productParameters.PageNumber, productParameters.PageSize, productParameters.Search))
            .Returns(new PagedList<Product>(new List<Product>
        {
            product
        }, 1, 1, 1));
        
        // Act
        var foundProducts = _productService.GetProducts(productParameters, out var metadata);
        // Assert
        Assert.Equal(1, foundProducts.Count);
    }
}