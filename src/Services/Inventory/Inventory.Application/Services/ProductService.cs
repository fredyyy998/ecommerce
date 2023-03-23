using System.Collections;
using AutoMapper;
using Inventory.Application.Dtos;
using Inventory.Core.Product;

namespace Inventory.Application.Services;

public class ProductService : IProductService
{
    private readonly IMapper _mapper;
    private readonly IProductRepository _productRepository;
    
    public ProductService(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public ICollection<ProductResponseDto> SearchProduct(string searchString)
    {
        var products = _productRepository.Search(searchString);
        return _mapper.Map<ICollection<Product>, List<ProductResponseDto>>(products);
    }

    public void ReserveProduct(Guid productId, int quantity)
    {
        var product = _productRepository.GetById(productId);
        product.RemoveStock(quantity);
    }

    public void CancelReservation(Guid productId, int quantity)
    {
        var product = _productRepository.GetById(productId);
        product.AddStock(quantity);
    }

    public void UpdateProduct(Guid productId, ProductUpdateDto productUpdateDto)
    {
        var product = _productRepository.GetById(productId);
        product.Update(productUpdateDto.Name, productUpdateDto.Description, productUpdateDto.GrossPrice);
    }

    public void CreateProduct(ProductCreateDto productCreateDto)
    {
        var product = Product.Create(productCreateDto.Name, productCreateDto.Description, productCreateDto.GrossPrice);
        _productRepository.Create(product);
    }
    
    public void AddStock(Guid productId, int quantity)
    {
        var product = _productRepository.GetById(productId);
        product.AddStock(quantity);
        _productRepository.Update(product);
    }
    
    public void RemoveStock(Guid productId, int quantity)
    {
        var product = _productRepository.GetById(productId);
        product.RemoveStock(quantity);
        _productRepository.Update(product);
    }

    public void DeleteProduct(Guid productId)
    {
        _productRepository.Delete(productId);
    }
}