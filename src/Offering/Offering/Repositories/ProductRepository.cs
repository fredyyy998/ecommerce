using Microsoft.EntityFrameworkCore;
using Offering.Models;

namespace Offering.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly DataContext _context;
    
    public ProductRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<Product> FindById(Guid id)
    {
        return await _context.Products.FindAsync(id);
    }

    public async Task<List<Product>> FindAll()
    {
        return await _context.Products.ToListAsync();
    }

    public async Task<Product> Add(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return await FindById(product.Id);
    }

    public Task Update(Product product)
    {
        _context.Products.Update(product);
        return _context.SaveChangesAsync();
    }

    public Task Delete(Product product)
    {
        _context.Products.Remove(product);
        return _context.SaveChangesAsync();
    }
}