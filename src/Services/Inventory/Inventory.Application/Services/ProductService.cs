using AutoMapper;
using Ecommerce.Common.Core;
using Inventory.Application.Dtos;
using Inventory.Application.Exceptions;
using Inventory.Core.DomainEvents;
using Inventory.Core.Product;
using Inventory.Infrastructure.Repository;
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

    public async Task<ProductResponseDto> GetProduct(Guid productId)
    {
        var product = await GetProductFromRepository(productId);
        return _mapper.Map<Product, ProductResponseDto>(product);
    }

    public async Task<AdminProductResponseDto> GetAdminProduct(Guid productId)
    {
        var product = await GetProductFromRepository(productId);
        return _mapper.Map<Product, AdminProductResponseDto>(product);
    }

    public async Task<Tuple<PagedList<ProductResponseDto>, object>> GetProducts(ProductParameters productParameters)
    {
        var products = await _productRepository.FindAll(productParameters, productParameters.Search);
        return await GetPagedResponse<ProductResponseDto>(products);
    }

    public async Task<Tuple<PagedList<AdminProductResponseDto>, object>> GetAdminProducts(ProductParameters productParameters)
    {
        var products = await _productRepository.FindAll(productParameters);
        return await GetPagedResponse<AdminProductResponseDto>(products);
    }
    
    private async Task<Tuple<PagedList<T>, object>> GetPagedResponse<T>(PagedList<Product> products)
    {
        var metadata = new
        {
            products.TotalCount,
            products.PageSize,
            products.CurrentPage,
            products.TotalPages,
            products.HasNext,
            products.HasPrevious
        };
        
        return new Tuple<PagedList<T>, object>(_mapper.Map<PagedList<Product>, PagedList<T>>(products), metadata);
    }

    public async Task UpdateProduct(Guid productId, ProductUpdateDto productUpdateDto)
    {
        var product = await GetProductFromRepository(productId);
        product.Update(productUpdateDto.Name, productUpdateDto.Description, productUpdateDto.GrossPrice);
        // remove all existing information
        product.Informations.ToList().ForEach(x => product.RemoveInformation(x.Key));
        foreach (var keyValuePair in productUpdateDto.ProductInformation)
        {
            product.AddInformation(keyValuePair.Key, keyValuePair.Value);
        }
        await _productRepository.Update(product);
    }

    public async Task<AdminProductResponseDto> CreateProduct(ProductCreateDto productCreateDto)
    {
        var product = Product.Create(productCreateDto.Name, productCreateDto.Description, productCreateDto.GrossPrice);
        product = await _productRepository.Create(product);
        return _mapper.Map<Product, AdminProductResponseDto>(product);
    }
    
    public async Task AddStock(Guid productId, int quantity)
    {
        var product = await GetProductFromRepository(productId);
        product.AddStock(quantity);
        await _productRepository.Update(product);
    }
    
    public async Task RemoveStock(Guid productId, int quantity)
    {
        var product = await GetProductFromRepository(productId);
        product.RemoveStock(quantity);
        await _productRepository.Update(product);
    }

    public async Task DeleteProduct(Guid productId)
    {
        await _productRepository.Delete(productId);
        // TODO this is a bit of an edge case, usually we would like to create the event in the product and public it after save
        // but in this case the product does not exist anymore and the event does not exist anymore aswell, so for now we simply publish here
        _mediator.Publish(new ProductRemovedByAdmin(productId));
    }
    
    private async Task<Product> GetProductFromRepository(Guid productId)
    {
        var product = await _productRepository.GetById(productId);
        if (product == null)
        {
            throw new EntityNotFoundException($"Product with id: {productId} not found");
        }
        return product;
    }
}