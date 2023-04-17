using Inventory.Core.Product;
using Inventory.Core.Utility;

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

    public PagedList<Product> GetAvailableProducts(int pageNumber, int pageSize, string? search)
    {
        // query for products with stock
        var query = _context.Products.Where(p => p.Stock > 0);
        if (search != null)
        {
            query = query.Where(p => p.Name.Contains(search) || p.Description.Contains(search));
        }
        return PagedList<Product>.ToPagedList(query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize),
                pageNumber, pageSize);
    }
}