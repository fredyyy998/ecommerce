using Ecommerce.Common.Core;

namespace Inventory.Core.DomainEvents;

public class ProductUpdatedByAdmin : IDomainEvent
{
    public Product.Product Product { get; }
    
    public ProductUpdatedByAdmin(Product.Product product)
    {
        Product = product;
    }
}