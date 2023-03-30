using Ecommerce.Common.Core;
using Inventory.Core.DomainEvents;

namespace Inventory.Core.Product;

public class Product : EntityRoot
{
    public string Name { get; private set; }
    
    public string Description { get; private set; }
    
    public Price Price { get; private set; }

    private List<ProductInformation> _informations = new ();
    
    public IReadOnlyCollection<ProductInformation> Information => _informations.AsReadOnly();
    
    public int Stock { get; set; }

    public static Product Create(string name, string description, decimal price)
    {
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = description,
            Price = Price.CreateDefault(price),
            Stock = 0,
        };

        product.AddDomainEvent(new ProductAddedByAdminEvent(product));
        return product;
    }

    public void AddInformation(string testKey, string testValue)
    {
        if (_informations.Any(x => x.Key == testKey))
        {
            throw new InvalidOperationException("The key already exists");
        }

        _informations.Add(ProductInformation.Create(testKey, testValue));
        
        AddDomainEvent(new ProductUpdatedByAdminEvent(this));
    }

    public void RemoveInformation(string key)
    {
        _informations.Remove(_informations.First(x => x.Key == key));
        
        AddDomainEvent(new ProductUpdatedByAdminEvent(this));
    }

    public void Update(string newName, string newDescription, decimal grossPrice)
    {
        Name = newName;
        Description = newDescription;
        Price.UpdateGross(grossPrice);
        
        AddDomainEvent(new ProductUpdatedByAdminEvent(this));
    }

    public void AddStock(int value)
    {
        if (value < 0)
        {
            throw new ProductDomainException($"Add to stock value cannot be negative");
        }
        
        Stock += value;
        
        AddDomainEvent(new ProductStockUpdated(this));
    }

    public void RemoveStock(int value)
    {
        if (value < 0)
        {
            throw new ProductDomainException($"Add to stock value cannot be negative");
        }

        if (Stock - value < 0)
        {
            throw new ProductDomainException($"Not enough stock for the product: {Name}");
        }
        
        Stock -= value;
        
        AddDomainEvent(new ProductStockUpdated(this));
    }
}