using Ecommerce.Common.Core;
using Fulfillment.Core.Order;

namespace Fulfillment.Core.DomainEvents;

public class OrderCancelledEvent : IDomainEvent
{
    public Guid BuyerId { get; }
    
    public Guid OrderId { get; }

    private List<OrderItem> _items;
    
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();
    
    public OrderCancelledEvent(Order.Order order)
    {
        BuyerId = order.BuyerId;
        OrderId = order.Id;
        _items = order.Products.ToList();
    }
}