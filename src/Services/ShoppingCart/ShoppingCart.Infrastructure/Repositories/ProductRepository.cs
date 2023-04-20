using Ecommerce.Common.Core;
using ShoppingCart.Core.Product;

namespace ShoppingCart.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly DataContext _dataContext;
    
    public ProductRepository(DataContext dataContext)
    {
        _dataContext = dataContext;
    }
    
    public async Task<Product> GetById(Guid id)
    {
        return await _dataContext.Products.FindAsync(id);
    }

    public async Task<Product> Create(Product entity)
    {
        _dataContext.Products.Add(entity);
        await _dataContext.SaveChangesAsync();
        return await GetById(entity.Id);
    }

    public async Task Update(Product entity)
    {
        _dataContext.Products.Update(entity);
        await _dataContext.SaveChangesAsync();
    }

    public async Task Delete(Guid id)
    {
        var product = await GetById(id);
        _dataContext.Products.Remove(product);
        await _dataContext.SaveChangesAsync();
    }

    public Task<PagedList<Product>> FindAll(PaginationParameter paginationParameter)
    {
        throw new NotImplementedException();
    }
}