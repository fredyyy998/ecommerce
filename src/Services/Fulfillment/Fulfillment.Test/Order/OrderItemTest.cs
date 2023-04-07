using Fulfillment.Core.Order;

namespace Fulfillment.Test.OrderTest;

public class OrderItemTest
{
    [Fact]
    public void Calculates_Total_Price_On_Creation()
    {
        var price = new Price(10, 10, 19, "EUR");
        
        var orderItem = OrderItem.Create("Test", price, 2);
        
        Assert.Equal(20, orderItem.TotalPrice.GrossPrice);
        Assert.Equal(20, orderItem.TotalPrice.NetPrice);
        Assert.Equal(19, orderItem.TotalPrice.Tax);
    }
}