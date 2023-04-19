using Ecommerce.Common.Core;

namespace Inventory.Infrastructure.Repository;

public class ProductParameters : PaginationParameter
{
    public string? Search { get; set; }
}