using Fulfillment.Core.Buyer;
using Fulfillment.Core.Order;
using Fulfillment.Core.Revenue;

namespace Fulfillment.Test.Revenue;

public class RevenueTest
{
    [Fact]
    public void Revenue_Start_Time_Is_Set_To_The_Earliest_Ordered_Date()
    {
        var orders = new List<Order>();
        var shippingAddress = new Address("123 Main St", "Anytown", "CA", "12345");
        orders.Add(Order.Create(Guid.NewGuid(), shippingAddress));
        orders.Add(Order.Create(Guid.NewGuid(), shippingAddress));
        orders.Add(Order.Create(Guid.NewGuid(), shippingAddress));
        
        var revenueReport = RevenueReport.CreateRevenueReport(orders);
        
        Assert.Equal(orders.First().OrderDate, revenueReport.StartDate);
    }
    
    [Fact]
    public void Revenue_End_Time_Is_Set_To_The_Latest_Ordered_Date()
    {
        var orders = new List<Order>();
        var shippingAddress = new Address("123 Main St", "Anytown", "CA", "12345");
        orders.Add(Order.Create(Guid.NewGuid(), shippingAddress));
        orders.Add(Order.Create(Guid.NewGuid(), shippingAddress));
        orders.Add(Order.Create(Guid.NewGuid(), shippingAddress));
        
        var revenueReport = RevenueReport.CreateRevenueReport(orders);
        
        Assert.Equal(orders.Last().OrderDate, revenueReport.EndDate);
    }
    
    [Fact]
    public void Total_Revenue_Is_Net_Sum_Of_All_Orders()
    {
        var orders = new List<Order>();
        var shippingAddress = new Address("123 Main St", "Anytown", "CA", "12345");
        var order = Order.Create(Guid.NewGuid(), shippingAddress);
        order.AddOrderItem(OrderItem.Create(Guid.NewGuid(), "Test", 10, 9, "EUR", 10, 5));
        order.AddOrderItem(OrderItem.Create(Guid.NewGuid(), "Test", 20, 18, "EUR", 10, 5));
        orders.Add(order);
        
        var order2 = Order.Create(Guid.NewGuid(), shippingAddress);
        order2.AddOrderItem(OrderItem.Create(Guid.NewGuid(), "Test", 5, 4, "EUR", 10, 5));
        orders.Add(order2);

        var revenueReport = RevenueReport.CreateRevenueReport(orders);
        
        Assert.Equal(155, revenueReport.Revenue);
    }
}