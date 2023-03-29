using Ecommerce.Common.Core;

namespace ShoppingCart.Core.Events;

public class ShoppingBasketTimedOutEvent : IDomainEvent
{
    public ShoppingCart.ShoppingCart ShoppingCart { get; }
    
    public ShoppingBasketTimedOutEvent(ShoppingCart.ShoppingCart shoppingCart)
    {
        ShoppingCart = shoppingCart;
    }
}