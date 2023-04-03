using Ecommerce.Common.Core;

namespace ShoppingCart.Core.Events;

public class ProductRemovedByAdminEvent : IDomainEvent
{
    public Guid ProductId { get; }
    
    public ProductRemovedByAdminEvent(Guid productId)
    {
        ProductId = productId;
    }
}