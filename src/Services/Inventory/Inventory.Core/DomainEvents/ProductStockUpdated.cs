using Ecommerce.Common.Core;

namespace Inventory.Core.DomainEvents;

public class ProductStockUpdated : IDomainEvent
{
    public Product.Product Product { get; }
    
    public ProductStockUpdated(Product.Product product)
    {
        Product = product;
    }
}