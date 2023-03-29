using Ecommerce.Common.Core;
using ShoppingCart.Core.Exceptions;

namespace ShoppingCart.Core.Product;

public class Product : EntityRoot
{
    public string Name { get; private set; }
    
    public string Description { get; private set; }
    
    public Price Price { get; private set; }
    
    public int Stock { get; private set; }
    
    private Product(Guid id, string name, string description, Price price, int stock)
    {
        Id = id;
        Name = name;
        Description = description;
        Price = price;
        Stock = stock;
    }
    
    public static Product Create(Guid id, string name, string description, Price price, int stock)
    {
        if (stock <= 0)
        {
            throw new ProductDomainException("Stock must be greater than zero");
        }
        
        return new Product(id, name, description, price, stock);
    }
    
    public void Update(string name, string description, Price price, int stock)
    {
        if (stock <= 0)
        {
            throw new ProductDomainException("Stock must be greater than zero");
        }
        
        Name = name;
        Description = description;
        Price = price;
        Stock = stock;
    }

    public void RemoveStock(int quantity)
    {
        if (quantity <= 0)
        {
            throw new ProductDomainException("Quantity must be greater than zero");
        }
        
        if (Stock - quantity < 0)
        {
            throw new ProductDomainException("Stock must be greater than zero");
        }
        
        Stock -= quantity;
    }

    public void AddStock(int quantity)
    {
        if (quantity <= 0)
        {
            throw new ProductDomainException("Quantity must be greater than zero");
        }
        
        Stock += quantity;
    }
}