using System.Collections;
using Inventory.Application.Dtos;
using Inventory.Core.Product;
using Inventory.Core.Utility;

namespace Inventory.Application.Services;

public interface IProductService
{
    ProductResponseDto GetProduct(Guid productId);
    AdminProductResponseDto GetAdminProduct(Guid productId);
    PagedList<ProductResponseDto> GetProducts(ProductParameters productParameters, out object metadata);
    
    void ReserveProduct(Guid productId, int quantity);
    
    void CancelReservation(Guid productId, int quantity);

    void UpdateProduct(Guid productId, ProductUpdateDto productUpdateDto);
    
    void CreateProduct(ProductCreateDto productCreateDto);
    
    void DeleteProduct(Guid productId);
    
    void AddStock(Guid productId, int quantity);
    
    void RemoveStock(Guid productId, int quantity);
}