using Inventory.Core.Product;

namespace Inventory.Infrastructure.Repository;

public class ProductRepository : IProductRepository
{
    private readonly DataContext _context;
    
    public ProductRepository(DataContext context)
    {
        _context = context;
    }

    public Product GetById(Guid id)
    {
        return _context.Products.Find(id);
    }

    public void Create(Product entity)
    {
        _context.Products.Add(entity);
        _context.SaveChanges();
    }

    public void Update(Product entity)
    {
        _context.Products.Update(entity);
        _context.SaveChanges();
    }

    public void Delete(Guid id)
    {
        var product = GetById(id);
        _context.Products.Remove(product);
        _context.SaveChanges();
    }

    public ICollection<Product> Search(string search)
    {
        return _context.Products.Where(p => p.Name.Contains(search) || p.Description.Contains(search)).ToList();
    }
}