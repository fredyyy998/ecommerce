using System.Collections;
using Inventory.Application.Dtos;

namespace Inventory.Application.Services;

public interface IProductService
{
    ProductResponseDto GetProduct(Guid productId);
    ICollection<ProductResponseDto> SearchProduct(string searchString);
    
    void ReserveProduct(Guid productId, int quantity);
    
    void CancelReservation(Guid productId, int quantity);

    void UpdateProduct(Guid productId, ProductUpdateDto productUpdateDto);
    
    void CreateProduct(ProductCreateDto productCreateDto);
    
    void DeleteProduct(Guid productId);
    
    void AddStock(Guid productId, int quantity);
    
    void RemoveStock(Guid productId, int quantity);
}