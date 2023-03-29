using ShoppingCart.Core.Product;

namespace ShoppingCart.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly DataContext _dataContext;
    
    public ProductRepository(DataContext dataContext)
    {
        _dataContext = dataContext;
    }
    
    public Product GetById(Guid id)
    {
        return _dataContext.Products.Find(id);
    }

    public void Create(Product entity)
    {
        _dataContext.Products.Add(entity);
        _dataContext.SaveChanges();
    }

    public void Update(Product entity)
    {
        _dataContext.Products.Update(entity);
        _dataContext.SaveChanges();
    }

    public void Delete(Guid id)
    {
        var product = GetById(id);
        _dataContext.Products.Remove(product);
        _dataContext.SaveChanges();
    }
}