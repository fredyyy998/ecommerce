using Ecommerce.Common.Core;

namespace Inventory.Core.DomainEvents;

public class ProductRemovedByAdmin : IDomainEvent
{
    
    public Guid ProductId { get; }
    
    public ProductRemovedByAdmin(Guid productId)
    {
        ProductId = productId;
    }
}