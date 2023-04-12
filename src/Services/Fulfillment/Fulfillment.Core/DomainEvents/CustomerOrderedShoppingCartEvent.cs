using Ecommerce.Common.Core;
using Fulfillment.Core.Order;

namespace Fulfillment.Core.DomainEvents;

public class CustomerOrderedShoppingCartEvent : IDomainEvent
{
    public Guid ShoppingCartId { get; }
    
    public Guid CustomerId { get; }
    
    private List<OrderItem> _items;

    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();
    
    public DateTime CreatedAt { get; }

    public DateTime? UpdatedAt { get; }
    
    public CustomerOrderedShoppingCartEvent(Guid shoppingCartId, Guid customerId, List<OrderItem> items, DateTime createdAt, DateTime? updatedAt)
    {
        ShoppingCartId = shoppingCartId;
        CustomerId = customerId;
        _items = items;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }
}