using Inventory.Application.Dtos;

namespace Inventory.Application.Services;

public interface IProductService
{
    ICollection<ProductResponseDto> SearchProduct(string searchString);
    
    void ReserveProduct(Guid productId, int quantity);
    
    void CancelReservation(Guid productId, int quantity);

    void UpdateProduct(Guid productId, ProductUpdateDto productUpdateDto);
    
    void CreateProduct(ProductCreateDto productCreateDto);
    
    void DeleteProduct(Guid productId);
}