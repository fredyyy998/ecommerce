using Ecommerce.Common.Core;

namespace Inventory.Core.DomainEvents;

public class OrderCancelledEvent : IDomainEvent
{
    private List<OrderItemDto> _items;
    
    public IReadOnlyCollection<OrderItemDto> Items => _items.AsReadOnly();
    
    public OrderCancelledEvent(List<OrderItemDto> items)
    {
        _items = items;
    }
}

public class OrderItemDto
{
    public Guid ProductId { get; private set; }
    public int Quantity { get; private set; }
    
    public OrderItemDto(Guid productId, int quantity)
    {
        ProductId = productId;
        Quantity = quantity;
    }
}