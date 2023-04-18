using Ecommerce.Common.Core;

namespace Fulfillment.Core.DomainEvents;

public class CustomerOrderedShoppingCartEvent : IDomainEvent
{
    public Guid ShoppingCartId { get; }
    
    public Guid CustomerId { get; }
    
    private List<ShoppingCartItemDto> _items;

    public IReadOnlyCollection<ShoppingCartItemDto> Items => _items.AsReadOnly();
    
    public ShoppingCartCheckoutDto ShoppingCartCheckout { get; }
    
    public DateTime CreatedAt { get; }

    public DateTime? UpdatedAt { get; }
    
    public CustomerOrderedShoppingCartEvent(Guid shoppingCartId, Guid customerId, List<ShoppingCartItemDto> items, ShoppingCartCheckoutDto shoppingCartCheckout, DateTime createdAt, DateTime? updatedAt)
    {
        ShoppingCartId = shoppingCartId;
        CustomerId = customerId;
        _items = items;
        ShoppingCartCheckout = shoppingCartCheckout;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }
}