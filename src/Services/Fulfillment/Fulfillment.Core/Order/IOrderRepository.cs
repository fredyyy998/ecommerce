using Ecommerce.Common.Core;

namespace Fulfillment.Core.Order;

public interface IOrderRepository
{
    Task<Order> FindByIdAsync(Guid orderId);
    Task<List<Order>> FindAllByBuyerIdAsync(Guid buyerId);
    Task SaveAsync(Order order);
    Task UpdateAsync(Order order);
    Task<List<Order>> FindInDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<PagedList<Order>> FindReadyToShipAsync(PaginationParameter parameters, string? status);
}