using Ecommerce.Common.Core;
using Inventory.Core.Utility;

namespace Inventory.Core.Product;

public interface IProductRepository : IRepository<Product>
{
    PagedList<Product> GetAvailableProducts(int pageNumber, int pageSize, string? searchString);
}