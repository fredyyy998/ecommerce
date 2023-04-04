using Ecommerce.Common.Core;

namespace ShoppingCart.Core.Events;

public class ReservationCanceledDueToStockUpdateEvent : IDomainEvent
{
    public Guid ShoppingCartId { get; }
    
    public Guid ProductId { get; }
    
    public int Quantity { get; }
    
    public ReservationCanceledDueToStockUpdateEvent(Guid shoppingCartId, Guid productId, int quantity)
    {
        ShoppingCartId = shoppingCartId;
        ProductId = productId;
        Quantity = quantity;
    }
}