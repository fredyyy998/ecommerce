using Ecommerce.Common.Core;

namespace Inventory.Core.Product;

public interface IProductRepository : IRepository<Product>
{
    ICollection<Product> Search(string search);
}