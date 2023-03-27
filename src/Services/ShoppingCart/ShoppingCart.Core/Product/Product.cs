using Ecommerce.Common.Core;

namespace ShoppingCart.Core.Product;

public class Product : EntityRoot
{
    public string Name { get; private set; }
    
    public string Description { get; private set; }
    
    public Price Price { get; private set; }
    
    public int Stock { get; private set; }
    
    public Product(Guid id, string name, string description, Price price, int stock)
    {
        Id = id;
        Name = name;
        Description = description;
        Price = price;
        Stock = stock;
    }
    
    
}