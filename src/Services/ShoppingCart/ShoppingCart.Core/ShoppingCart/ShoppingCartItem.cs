using Ecommerce.Common.Core;
using ShoppingCart.Core.Exceptions;

namespace ShoppingCart.Core.ShoppingCart;

public class ShoppingCartItem : ValueObject
{
    public Product.Product Product { get; private set; }
    
    public int Quantity { get; private set; }
    
    public decimal TotalPrice { get; private set; }
    
    private ShoppingCartItem(Product.Product product, int quantity)
    {
        Product = product;
        Quantity = quantity;
        TotalPrice = Product.Price.GrossPrice * Quantity;
    }
    
    public static ShoppingCartItem Create(Product.Product product, int quantity)
    {
        if (quantity <= 0)
        {
            throw new ShoppingCartDomainException("Quantity must be greater than zero.");
        }
        
        return new ShoppingCartItem(product, quantity);
    }
    


    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Product;
        yield return Quantity;
    }

    public void IncreaseQuantity(int quantity)
    {
        if (quantity <= 0)
        {
            throw new ShoppingCartDomainException("Quantity must be greater than zero.");
        }
        
        Quantity += quantity;
        TotalPrice = Product.Price.GrossPrice * Quantity;
    }

    public void DecreaseQuantity(int quantity)
    {
        if (quantity <= 0)
        {
            throw new ShoppingCartDomainException("Quantity must be greater than zero.");
        }
        
        Quantity -= quantity;
        TotalPrice = Product.Price.GrossPrice * Quantity;
    }
}