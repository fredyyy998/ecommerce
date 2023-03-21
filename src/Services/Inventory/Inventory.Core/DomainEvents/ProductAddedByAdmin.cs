using Ecommerce.Common.Core;

namespace Inventory.Core.DomainEvents;

public class ProductAddedByAdmin : IDomainEvent
{
    public Product.Product Product { get; }
    
    public ProductAddedByAdmin(Product.Product product)
    {
        Product = product;
    }
}