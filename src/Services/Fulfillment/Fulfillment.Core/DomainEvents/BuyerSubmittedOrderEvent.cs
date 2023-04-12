using Ecommerce.Common.Core;
using Fulfillment.Core.Order;

namespace Fulfillment.Core.DomainEvents;

public class BuyerSubmittedOrderEvent : IDomainEvent
{
    Guid OrderId { get; }
    
    Guid BuyerId { get; }
    
    private List<OrderItem> _items { get; }
    
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();
    
    public BuyerSubmittedOrderEvent(Order.Order order)
    {
        OrderId = order.Id;
        BuyerId = order.BuyerId;
        _items = new List<OrderItem>(order.Products);
    }
}