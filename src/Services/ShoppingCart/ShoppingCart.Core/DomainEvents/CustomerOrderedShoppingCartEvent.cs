using Ecommerce.Common.Core;
using ShoppingCart.Core.ShoppingCart;

namespace ShoppingCart.Core.Events;

public class CustomerOrderedShoppingCartEvent : IDomainEvent
{
    public Guid ShoppingCartId { get; }
    
    public Guid CustomerId { get; }
    
    private List<ShoppingCartItem> _items;

    public IReadOnlyCollection<ShoppingCartItem> Items => _items.AsReadOnly();
    
    public DateTime CreatedAt { get; }

    public DateTime? UpdatedAt { get; }

    public CustomerOrderedShoppingCartEvent(ShoppingCart.ShoppingCart shoppingCart)
    {
        ShoppingCartId = shoppingCart.Id;
        CustomerId = shoppingCart.CustomerId;
        _items = shoppingCart.Items.ToList();
        CreatedAt = shoppingCart.CreatedAt;
        UpdatedAt = shoppingCart.UpdatedAt;
    }
}