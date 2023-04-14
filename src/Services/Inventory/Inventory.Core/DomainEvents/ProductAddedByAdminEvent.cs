using Ecommerce.Common.Core;
using Inventory.Core.Product;

namespace Inventory.Core.DomainEvents;

public class ProductAddedByAdminEvent : IDomainEvent
{
    public Guid Id { get; }
    
    public string Name { get; }
    
    public string Description { get; }
    
    public Price Price { get; }
    
    public IReadOnlyCollection<ProductInformation> Information { get; }
    
    public int Stock { get; set; }

    public ProductAddedByAdminEvent(Product.Product product)
    {
        Id = product.Id;
        Name = product.Name;
        Description = product.Description;
        Price = product.Price;
        Information = product.Informations;
        Stock = product.Stock;
    }
}