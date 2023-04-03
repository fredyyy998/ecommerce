using Ecommerce.Common.Core;

namespace ShoppingCart.Core.Events;

public class ProductStockUpdatedByAdminEvent : IDomainEvent
{
    public Guid ProductId { get; }
    
    public int Stock { get; }
    
    public ProductStockUpdatedByAdminEvent(Guid productId, int stock)
    {
        ProductId = productId;
        Stock = stock;
    }
}