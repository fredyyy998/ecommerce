using Fulfillment.Core.Buyer;
using Fulfillment.Core.Exceptions;
using Fulfillment.Core.Order;

namespace Fulfillment.Test.OrderTest;

public class OrderTest
{
    [Fact]
    public void Calculates_Total_Price_On_Adding_Order_Item()
    {
        var price = new Price(10, 10, 19, "EUR");
        var orderItem = new OrderItem("Test", price, 2);
        var order = CreateOrder();

        order.AddOrderItem(orderItem);
        
        Assert.Equal(20, order.TotalPrice.GrossPrice);
        Assert.Equal(20, order.TotalPrice.NetPrice);
    }

    [Fact]
    public void OrderItems_Can_Be_Added_In_Created_Stage()
    {
        var price = new Price(10, 10, 19, "EUR");
        var orderItem = new OrderItem("Test", price, 2);
        var order = CreateOrder();
        
        order.AddOrderItem(orderItem);
        
        Assert.Single(order.Products);
    }

    #region ChangeStateTests

    [Fact]
    public void Does_Not_Allow_State_Change_From_Created_To_Paid()
    {
        var order = CreateOrder();

        Assert.Throws<OrderDomainException>(() => order.ChangeState(OrderState.Paid));
    }
    
    [Fact]
    public void Does_Not_Allow_State_Change_From_Created_To_Delivered()
    {
        var order = CreateOrder();

        Assert.Throws<OrderDomainException>(() => order.ChangeState(OrderState.Delivered));
    }
    
    [Fact]
    public void Does_Not_Allow_State_Change_From_Created_To_Shipped()
    {
        var order = CreateOrder();

        Assert.Throws<OrderDomainException>(() => order.ChangeState(OrderState.Shipped));
    }

    [Fact]
    public void Does_Not_Allow_State_Change_From_Created_To_Created()
    {
        var order = CreateOrder();

        Assert.Throws<OrderDomainException>(() => order.ChangeState(OrderState.Created));
    }
    
    [Fact]
    public void Does_Not_Allow_State_Change_From_Submitted_To_Created()
    {
        var order = CreateOrder();
        order.ChangeState(OrderState.Submitted);

        Assert.Throws<OrderDomainException>(() => order.ChangeState(OrderState.Created));
    }
    
    [Fact]
    public void Does_Not_Allow_State_Change_From_Submitted_To_Shipped()
    {
        var order = CreateOrder();
        order.ChangeState(OrderState.Submitted);

        Assert.Throws<OrderDomainException>(() => order.ChangeState(OrderState.Shipped));
    }
    
    [Fact]
    public void Does_Not_Allow_State_Change_From_Submitted_To_Delivered()
    {
        var order = CreateOrder();
        order.ChangeState(OrderState.Submitted);

        Assert.Throws<OrderDomainException>(() => order.ChangeState(OrderState.Delivered));
    }
    
    [Fact]
    public void Does_Not_Allow_State_Change_From_Submitted_To_Submitted()
    {
        var order = CreateOrder();
        order.ChangeState(OrderState.Submitted);

        Assert.Throws<OrderDomainException>(() => order.ChangeState(OrderState.Submitted));
    }
    
    [Fact]
    public void Does_Not_Allow_State_Change_From_Paid_To_Created()
    {
        var order = CreateOrder();
        order.ChangeState(OrderState.Submitted);
        order.ChangeState(OrderState.Paid);

        Assert.Throws<OrderDomainException>(() => order.ChangeState(OrderState.Created));
    }
    
    [Fact]
    public void Does_Not_Allow_State_Change_From_Paid_To_Submitted()
    {
        var order = CreateOrder();
        order.ChangeState(OrderState.Submitted);
        order.ChangeState(OrderState.Paid);

        Assert.Throws<OrderDomainException>(() => order.ChangeState(OrderState.Submitted));
    }
    
    [Fact]
    public void Does_Not_Allow_State_Change_From_Paid_To_Delivered()
    {
        var order = CreateOrder();
        order.ChangeState(OrderState.Submitted);
        order.ChangeState(OrderState.Paid);

        Assert.Throws<OrderDomainException>(() => order.ChangeState(OrderState.Delivered));
    }
    
    [Fact]
    public void Does_Not_Allow_State_Change_From_Paid_To_Paid()
    {
        var order = CreateOrder();
        order.ChangeState(OrderState.Submitted);
        order.ChangeState(OrderState.Paid);

        Assert.Throws<OrderDomainException>(() => order.ChangeState(OrderState.Paid));
    }

    [Fact]
    public void Does_Not_Allow_State_Change_From_Shipped_To_Created()
    {
        var order = CreateOrder();
        order.ChangeState(OrderState.Submitted);
        order.ChangeState(OrderState.Paid);
        order.ChangeState(OrderState.Shipped);

        Assert.Throws<OrderDomainException>(() => order.ChangeState(OrderState.Created));
    }
    
    [Fact]
    public void Does_Not_Allow_State_Change_From_Shipped_To_Submitted()
    {
        var order = CreateOrder();
        order.ChangeState(OrderState.Submitted);
        order.ChangeState(OrderState.Paid);
        order.ChangeState(OrderState.Shipped);

        Assert.Throws<OrderDomainException>(() => order.ChangeState(OrderState.Submitted));
    }
    
    [Fact]
    public void Does_Not_Allow_State_Change_From_Shipped_To_Paid()
    {
        var order = CreateOrder();
        order.ChangeState(OrderState.Submitted);
        order.ChangeState(OrderState.Paid);
        order.ChangeState(OrderState.Shipped);

        Assert.Throws<OrderDomainException>(() => order.ChangeState(OrderState.Paid));
    }
    
    [Fact]
    public void Does_Not_Allow_State_Change_From_Shipped_To_Shipped()
    {
        var order = CreateOrder();
        order.ChangeState(OrderState.Submitted);
        order.ChangeState(OrderState.Paid);
        order.ChangeState(OrderState.Shipped);

        Assert.Throws<OrderDomainException>(() => order.ChangeState(OrderState.Shipped));
    }
    
    [Theory]
    [InlineData(OrderState.Created)]
    [InlineData(OrderState.Submitted)]
    [InlineData(OrderState.Paid)]
    [InlineData(OrderState.Shipped)]
    [InlineData(OrderState.Delivered)]
    [InlineData(OrderState.Cancelled)]
    public void Does_Not_Allow_State_Change_From_Delivered_To_any_other(OrderState state)
    {
        var order = CreateOrder();
        order.ChangeState(OrderState.Submitted);
        order.ChangeState(OrderState.Paid);
        order.ChangeState(OrderState.Shipped);
        order.ChangeState(OrderState.Delivered);

        Assert.Throws<OrderDomainException>(() => order.ChangeState(state));
    }

    #endregion
    
    private Order CreateOrder()
    {
        return Order.Create(Guid.NewGuid());
    }
}