using Ecommerce.Common.Core;

namespace Inventory.Core.DomainEvents;

public class ProductStockUpdated : IDomainEvent
{
    public Guid productId { get; }
    
    public int Stock { get; }
    
    public ProductStockUpdated(Product.Product product)
    {
        productId = product.Id;
        Stock = product.Stock;
    }
}