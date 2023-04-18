using Ecommerce.Common.Core;

namespace Inventory.Core.DomainEvents;

public class CustomerOrderedShoppingCartEvent : IDomainEvent
{
    private List<ShoppingCartItemDto> _items;

    public IReadOnlyCollection<ShoppingCartItemDto> Items => _items.AsReadOnly();
    
    public CustomerOrderedShoppingCartEvent(List<ShoppingCartItemDto> items)
    {
        _items = items;
    }
}

public class ShoppingCartItemDto
{
    public Guid ProductId { get; private set; }
    public int Quantity { get; private set; }

    public ShoppingCartItemDto(Guid productId, int quantity)
    {
        ProductId = productId;
        Quantity = quantity;
    }
}