using Ecommerce.Common.Core;
using ShoppingCart.Core.Product;

namespace ShoppingCart.Core.Events;

public class ProductAddedByAdminEvent : IDomainEvent
{
    public Guid Id { get; }
    
    public string Name { get; }
    
    public string Description { get;  }
    
    public Price Price { get; }

    public int Stock { get; }

    public ProductAddedByAdminEvent(Guid id, string name, string description, Price price, int stock)
    {
        Id = id;
        Name = name;
        Description = description;
        Price = price;
        Stock = stock;
    }
}