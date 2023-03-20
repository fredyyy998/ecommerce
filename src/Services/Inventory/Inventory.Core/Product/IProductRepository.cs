namespace Inventory.Core.Product;

public interface IProductRepository
{
    void Create(Product customer);
    Product GetById(Guid id);
    ICollection<Product> Search(string search);
    void Update(Product customer);
    void Delete(Guid id);
}