using Fulfillment.Core.Order;
using Microsoft.EntityFrameworkCore;

namespace Fulfillment.Infrastructure.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly DataContext _dataContext;
    
    public OrderRepository (DataContext dataContext)
    {
        _dataContext = dataContext;
    }
    
    public Task<Order> FindByIdAsync(Guid orderId)
    {
        return _dataContext.Orders.FindAsync(orderId).AsTask();
    }

    public Task<List<Order>> FindAllByBuyerIdAsync(Guid buyerId)
    {
        return _dataContext.Orders.Where(o => o.BuyerId == buyerId).ToListAsync();
    }

    public Task SaveAsync(Order order)
    {
        _dataContext.Orders.Add(order);
        return _dataContext.SaveChangesAsync();
    }

    public Task UpdateAsync(Order order)
    {
        _dataContext.Orders.Update(order);
        return _dataContext.SaveChangesAsync();
    }

    public Task<List<Order>> FindInDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return _dataContext.Orders.Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate).ToListAsync();
    }
}