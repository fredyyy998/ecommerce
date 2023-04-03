using Ecommerce.Common.Core;

namespace ShoppingCart.Core.Events;

public class ShoppingCartTimedOutEvent : IDomainEvent
{
    public ShoppingCart.ShoppingCart ShoppingCart { get; }
    
    public ShoppingCartTimedOutEvent(ShoppingCart.ShoppingCart shoppingCart)
    {
        ShoppingCart = shoppingCart;
    }
}