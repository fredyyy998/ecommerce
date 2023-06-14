namespace Offering.Models;

public class Product
{
    public Guid Id { get; private set; }
    
    public string Name { get; private set; }
    
    public string Description { get; private set; }
    
    protected Product(Guid id, string name, string description)
    {
        Id = id;
        Name = name;
        Description = description;
    }
    
    public static Product Create(Guid id, string name, string description)
    {
        return new Product(id, name, description);
    }
}