using AutoMapper;
using Ecommerce.Common.Core;
using Inventory.Application.Services;
using Inventory.Application.Utils;
using Inventory.Core.Product;
using Inventory.Infrastructure.Repository;
using MediatR;
using Moq;

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
    public async Task Product_search_response_with_list()
    {
        // Arrange
        var product = Product.Create("test", "test", 1);
        var productParameters = new ProductParameters();
        productParameters.Search = "test";
        productParameters.PageNumber = 1;
        productParameters.PageSize = 20;
        _productRepositoryMock.Setup(x => x.FindAllAvailable(productParameters))
            .ReturnsAsync(new PagedList<Product>(new List<Product>
        {
            product
        }, 1, 1, 1));
        
        // Act
        var foundProducts = await _productService.GetProducts(productParameters);
        // Assert
        Assert.Equal(1, foundProducts.Item1.Count);
    }
}