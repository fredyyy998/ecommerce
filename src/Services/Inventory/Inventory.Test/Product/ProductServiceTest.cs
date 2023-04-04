using System.Collections.ObjectModel;
using AutoMapper;
using Inventory.Application.Services;
using Inventory.Application.Utils;
using Inventory.Core.Product;
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
        _productRepositoryMock.Setup(x => x.Search("test")).Returns(new List<Product> {product});
        
        // Act
        var foundProducts = _productService.SearchProduct("test");
        // Assert
        Assert.Equal(1, foundProducts.Count);
    }
}