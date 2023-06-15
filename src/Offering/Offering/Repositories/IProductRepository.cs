using Offering.Models;

namespace Offering.Repositories;

public interface IProductRepository
{
    Task<Product> FindById(Guid id);
    
    Task<List<Product>> FindAll();
    
    Task<Product> Add(Product product);
    
    Task Update(Product product);
    
    Task Delete(Product product);
}