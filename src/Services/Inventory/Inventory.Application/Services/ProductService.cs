using System.Collections;
using AutoMapper;
using Inventory.Application.Dtos;
using Inventory.Application.Exceptions;
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

    public ProductResponseDto GetProduct(Guid productId)
    {
        var product = GetProductFromRepository(productId);
        return _mapper.Map<Product, ProductResponseDto>(product);
    }

    public ICollection<ProductResponseDto> SearchProduct(string searchString)
    {
        var products = _productRepository.Search(searchString);
        return _mapper.Map<ICollection<Product>, List<ProductResponseDto>>(products);
    }

    public void ReserveProduct(Guid productId, int quantity)
    {
        var product = GetProductFromRepository(productId);
        product.RemoveStock(quantity);
    }

    public void CancelReservation(Guid productId, int quantity)
    {
        var product = GetProductFromRepository(productId);
        product.AddStock(quantity);
    }

    public void UpdateProduct(Guid productId, ProductUpdateDto productUpdateDto)
    {
        var product = GetProductFromRepository(productId);
        product.Update(productUpdateDto.Name, productUpdateDto.Description, productUpdateDto.GrossPrice);
        _productRepository.Update(product);
    }

    public void CreateProduct(ProductCreateDto productCreateDto)
    {
        var product = Product.Create(productCreateDto.Name, productCreateDto.Description, productCreateDto.GrossPrice);
        _productRepository.Create(product);
    }
    
    public void AddStock(Guid productId, int quantity)
    {
        var product = GetProductFromRepository(productId);
        product.AddStock(quantity);
        _productRepository.Update(product);
    }
    
    public void RemoveStock(Guid productId, int quantity)
    {
        var product = GetProductFromRepository(productId);
        product.RemoveStock(quantity);
        _productRepository.Update(product);
    }

    public void DeleteProduct(Guid productId)
    {
        _productRepository.Delete(productId);
    }
    
    private Product GetProductFromRepository(Guid productId)
    {
        var product = _productRepository.GetById(productId);
        if (product == null)
        {
            throw new EntityNotFoundException($"Product with id: {productId} not found");
        }
        return product;
    }
}