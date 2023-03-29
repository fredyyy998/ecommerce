using Ecommerce.Common.Core;

namespace ShoppingCart.Core.Events;

public class CustomerAddedProductToBasketEvent : IDomainEvent
{
    
    public Guid ShoppingCartId { get; }
    
    public Product.Product Product { get; }
    
    public int Quantity { get; }
    
    public CustomerAddedProductToBasketEvent(Product.Product product, int quantity)
    {
        Product = product;
        Quantity = quantity;
    }
}