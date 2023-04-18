using Ecommerce.Common.Core;
using ShoppingCart.Core.ShoppingCart;

namespace ShoppingCart.Core.Events;

public class CustomerOrderedShoppingCartEvent : IDomainEvent
{
    public Guid ShoppingCartId { get; }
    
    public Guid CustomerId { get; }
    
    private List<ShoppingCartItemDto> _items;

    public IReadOnlyCollection<ShoppingCartItemDto> Items => _items.AsReadOnly();
    
    public ShoppingCartCheckout ShoppingCartCheckout { get; }
    
    public DateTime CreatedAt { get; }

    public DateTime? UpdatedAt { get; }

    public CustomerOrderedShoppingCartEvent(ShoppingCart.ShoppingCart shoppingCart)
    {
        ShoppingCartId = shoppingCart.Id;
        CustomerId = shoppingCart.CustomerId;
        CreatedAt = shoppingCart.CreatedAt;
        UpdatedAt = shoppingCart.UpdatedAt;
        _items = shoppingCart.Items.Select(ShoppingCartItemDto.FromShoppingCartItem).ToList();
        ShoppingCartCheckout = shoppingCart.ShoppingCartCheckout;
    }
}

public class ShoppingCartItemDto
{
    public Guid ProductId { get; private set; }
    public string Name { get; private set; }
    public decimal NetPrice { get; private set; }
    public decimal GrossPrice { get; private set; }
    public string CurrencyCode { get; private set; }
    public int Quantity { get; private set; }
    public decimal TotalPrice { get; private set; }
    
    public static ShoppingCartItemDto FromShoppingCartItem(ShoppingCartItem shoppingCartItem)
    {
        return new ShoppingCartItemDto
        {
            ProductId = shoppingCartItem.Product.Id,
            Name = shoppingCartItem.Product.Name,
            NetPrice = shoppingCartItem.Product.Price.NetPrice,
            GrossPrice = shoppingCartItem.Product.Price.GrossPrice,
            CurrencyCode = shoppingCartItem.Product.Price.CurrencyCode,
            Quantity = shoppingCartItem.Quantity,
            TotalPrice = shoppingCartItem.TotalPrice
        };
    }
}