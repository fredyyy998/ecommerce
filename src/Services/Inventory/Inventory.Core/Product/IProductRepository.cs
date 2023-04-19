﻿using Ecommerce.Common.Core;

namespace Inventory.Core.Product;

public interface IProductRepository : IRepository<Product>
{
    Task<PagedList<Product>> FindAllAvailable(PaginationParameter paginationParameter);
}