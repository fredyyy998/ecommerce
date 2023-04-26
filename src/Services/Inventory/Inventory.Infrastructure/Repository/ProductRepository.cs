using Ecommerce.Common.Core;
using Inventory.Core.Product;

namespace Inventory.Infrastructure.Repository;

public class ProductRepository : IProductRepository
{
    private readonly DataContext _context;
    
    public ProductRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<Product> GetById(Guid id)
    {
        return await _context.Products.FindAsync(id);
    }

    public async Task<Product> Create(Product entity)
    {
        _context.Products.Add(entity);
        await _context.SaveChangesAsync();
        return  await GetById(entity.Id);
    }

    public async Task Update(Product entity)
    {
        _context.Products.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(Guid id)
    {
        var product = await GetById(id);
        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
    }

    public async Task<PagedList<Product>> FindAll(PaginationParameter paginationParameter, string search)
    {
        // query for products with stock
        var query = await GetAvailableProductsSearchQuery(paginationParameter, search);
        return GetPagedListFromQuery(query, paginationParameter);
    }

    public async Task<PagedList<Product>> FindAll(PaginationParameter paginationParameter)
    {
        // query for products with stock
        var query = _context.Products.AsQueryable();
        return GetPagedListFromQuery(query, paginationParameter);
    }

    public async Task<PagedList<Product>> FindAllAvailable(PaginationParameter paginationParameter)
    {
        var query = await GetAvailableProductsQuery();
        return GetPagedListFromQuery(query, paginationParameter);
    }

    private async Task<IQueryable<Product>> GetAvailableProductsQuery()
    {
        var query = _context.Products.AsQueryable();
        query = query.Where(p => p.Stock > 0);
        return query;
    }
    
    private async Task<IQueryable<Product>> GetAvailableProductsSearchQuery(PaginationParameter productParameters, string search)
    {
        var query = await GetAvailableProductsQuery();
        if (!string.IsNullOrWhiteSpace(search))
        {
            var searchTrimmed = search.Trim();
            query = query.Where(p => p.Name.Contains(searchTrimmed) || p.Description.Contains(searchTrimmed));
        }
        return query;
    }

    private PagedList<Product> GetPagedListFromQuery(IQueryable<Product> query,
        PaginationParameter paginationParameter)
    {
        return PagedList<Product>.ToPagedList(query
                .Skip((paginationParameter.PageNumber - 1) * paginationParameter.PageSize)
                .Take(paginationParameter.PageSize),
            paginationParameter.PageNumber, paginationParameter.PageSize);
    }
}