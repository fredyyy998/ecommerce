using Ecommerce.Common.Core;

namespace ShoppingCart.Core.Events;

public class CustomerOrderedShoppingCartEvent : IDomainEvent
{
    public ShoppingCart.ShoppingCart ShoppingCart { get; }
    
    public CustomerOrderedShoppingCartEvent(ShoppingCart.ShoppingCart shoppingCart)
    {
        ShoppingCart = shoppingCart;
    }
}