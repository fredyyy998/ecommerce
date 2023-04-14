using System.Collections;
using AutoMapper;
using Inventory.Application.Dtos;
using Inventory.Application.Exceptions;
using Inventory.Core.DomainEvents;
using Inventory.Core.Product;
using MediatR;

namespace Inventory.Application.Services;

public class ProductService : IProductService
{
    private readonly IMapper _mapper;
    private readonly IProductRepository _productRepository;
    private readonly IMediator _mediator;
    
    public ProductService(IProductRepository productRepository, IMapper mapper, IMediator mediator)
    {
        _productRepository = productRepository;
        _mapper = mapper;
        _mediator = mediator;
    }

    public ProductResponseDto GetProduct(Guid productId)
    {
        var product = GetProductFromRepository(productId);
        return _mapper.Map<Product, ProductResponseDto>(product);
    }

    public AdminProductResponseDto GetAdminProduct(Guid productId)
    {
        var product = GetProductFromRepository(productId);
        return _mapper.Map<Product, AdminProductResponseDto>(product);
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
        // remove all existing information
        product.Informations.ToList().ForEach(x => product.RemoveInformation(x.Key));
        foreach (var keyValuePair in productUpdateDto.ProductInformation)
        {
            product.AddInformation(keyValuePair.Key, keyValuePair.Value);
        }
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
        // TODO this is a bit of an edge case, usually we would like to create the event in the product and public it after save
        // but in this case the product does not exist anymore and the event does not exist anymore aswell, so for now we simply publish here
        _mediator.Publish(new ProductRemovedByAdmin(productId));
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