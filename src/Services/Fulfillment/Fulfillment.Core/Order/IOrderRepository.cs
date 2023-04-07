namespace Fulfillment.Core.Order;

public interface IOrderRepository
{
    Task<Order> FindByIdAsync(Guid orderId);
    Task<IEnumerable<Order>> FindAllByBuyerIdAsync(Guid buyerId);
    Task SaveAsync(Order order);
    Task UpdateAsync(Order order);
}