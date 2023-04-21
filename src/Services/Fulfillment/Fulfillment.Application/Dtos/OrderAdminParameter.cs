using Ecommerce.Common.Core;

namespace Fulfillment.Application.Dtos;

public class OrderAdminParameter : PaginationParameter
{
    public string? Status { get; set; }
}