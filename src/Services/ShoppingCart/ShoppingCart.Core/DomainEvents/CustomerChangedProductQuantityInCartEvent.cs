using Ecommerce.Common.Core;

namespace ShoppingCart.Core.Events;

public class CustomerChangedProductQuantityInCartEvent : IDomainEvent
{
    public Guid ShoppingCartId { get; }
    
    public Product.Product Product { get; }
    
    public int NewQuantity { get; }
    
    public CustomerChangedProductQuantityInCartEvent(Guid shoppingCartId, Product.Product product, int newQuantity)
    {
        ShoppingCartId = shoppingCartId;
        Product = product;
        NewQuantity = newQuantity;
    }
}