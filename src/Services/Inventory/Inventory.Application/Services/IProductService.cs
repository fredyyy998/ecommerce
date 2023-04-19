using Ecommerce.Common.Core;
using Inventory.Application.Dtos;
using Inventory.Infrastructure.Repository;

namespace Inventory.Application.Services;

public interface IProductService
{
    Task<ProductResponseDto> GetProduct(Guid productId);
    Task<AdminProductResponseDto> GetAdminProduct(Guid productId);
    Task<Tuple<PagedList<ProductResponseDto>, object>> GetProducts(ProductParameters productParameters);
    
    Task<Tuple<PagedList<AdminProductResponseDto>, object>> GetAdminProducts(ProductParameters productParameters);

    Task UpdateProduct(Guid productId, ProductUpdateDto productUpdateDto);
    
    Task<AdminProductResponseDto> CreateProduct(ProductCreateDto productCreateDto);
    
    Task DeleteProduct(Guid productId);
    
    Task AddStock(Guid productId, int quantity);
    
    Task RemoveStock(Guid productId, int quantity);
}