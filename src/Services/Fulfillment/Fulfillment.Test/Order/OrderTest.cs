using Fulfillment.Core.Buyer;
using Fulfillment.Core.DomainEvents;
using Fulfillment.Core.Exceptions;
using Fulfillment.Core.Order;

namespace Fulfillment.Test.OrderTest;

public class OrderTest
{
    [Fact]
    public void Calculates_Total_Price_On_Adding_Order_Item()
    {
        var price = new Price(10, 10, 19, "EUR");
        var orderItem = OrderItem.Create(Guid.NewGuid(), "Test", price.GrossPrice, price.NetPrice, price.Currency,
            price.Tax, 2);
        var order = CreateOrder();

        order.AddOrderItem(orderItem);
        
        Assert.Equal(20, order.TotalPrice.GrossPrice);
        Assert.Equal(20, order.TotalPrice.NetPrice);
    }

    [Fact]
    public void OrderItems_Can_Be_Added_In_Created_Stage()
    {
        var price = new Price(10, 10, 19, "EUR");
        var orderItem = OrderItem.Create(Guid.NewGuid(), "Test", price.GrossPrice, price.NetPrice, price.Currency, price.Tax,  2);
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
var order = CreateSubmittedOrder();

        Assert.Throws<OrderDomainException>(() => order.ChangeState(OrderState.Created));
    }
    
    [Fact]
    public void Does_Not_Allow_State_Change_From_Submitted_To_Shipped()
    {
var order = CreateSubmittedOrder();

        Assert.Throws<OrderDomainException>(() => order.ChangeState(OrderState.Shipped));
    }
    
    [Fact]
    public void Does_Not_Allow_State_Change_From_Submitted_To_Delivered()
    {
        var order = CreateSubmittedOrder();

        Assert.Throws<OrderDomainException>(() => order.ChangeState(OrderState.Delivered));
    }
    
    [Fact]
    public void Does_Not_Allow_State_Change_From_Submitted_To_Submitted()
    {
        var order = CreateSubmittedOrder();

        Assert.Throws<OrderDomainException>(() => order.ChangeState(OrderState.Submitted));
    }
    
    [Fact]
    public void Does_Not_Allow_State_Change_From_Paid_To_Created()
    {
        var order = CreateSubmittedOrder();
        order.ChangeState(OrderState.Paid);

        Assert.Throws<OrderDomainException>(() => order.ChangeState(OrderState.Created));
    }
    
    [Fact]
    public void Does_Not_Allow_State_Change_From_Paid_To_Submitted()
    {
        var order = CreateSubmittedOrder();
        order.ChangeState(OrderState.Paid);

        Assert.Throws<OrderDomainException>(() => order.ChangeState(OrderState.Submitted));
    }
    
    [Fact]
    public void Does_Not_Allow_State_Change_From_Paid_To_Delivered()
    {
        var order = CreateSubmittedOrder();
        order.ChangeState(OrderState.Paid);

        Assert.Throws<OrderDomainException>(() => order.ChangeState(OrderState.Delivered));
    }
    
    [Fact]
    public void Does_Not_Allow_State_Change_From_Paid_To_Paid()
    {
        var order = CreateSubmittedOrder();
        order.ChangeState(OrderState.Paid);

        Assert.Throws<OrderDomainException>(() => order.ChangeState(OrderState.Paid));
    }

    [Fact]
    public void Does_Not_Allow_State_Change_From_Shipped_To_Created()
    {
        var order = CreateSubmittedOrder();
        order.ChangeState(OrderState.Paid);
        order.ChangeState(OrderState.Shipped);

        Assert.Throws<OrderDomainException>(() => order.ChangeState(OrderState.Created));
    }
    
    [Fact]
    public void Does_Not_Allow_State_Change_From_Shipped_To_Submitted()
    {
        var order = CreateSubmittedOrder();
        order.ChangeState(OrderState.Paid);
        order.ChangeState(OrderState.Shipped);

        Assert.Throws<OrderDomainException>(() => order.ChangeState(OrderState.Submitted));
    }
    
    [Fact]
    public void Does_Not_Allow_State_Change_From_Shipped_To_Paid()
    {
        var order = CreateSubmittedOrder();
        order.ChangeState(OrderState.Paid);
        order.ChangeState(OrderState.Shipped);

        Assert.Throws<OrderDomainException>(() => order.ChangeState(OrderState.Paid));
    }
    
    [Fact]
    public void Does_Not_Allow_State_Change_From_Shipped_To_Shipped()
    {
        var order = CreateSubmittedOrder();
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
        var order = CreateSubmittedOrder();
        order.ChangeState(OrderState.Paid);
        order.ChangeState(OrderState.Shipped);
        order.ChangeState(OrderState.Delivered);

        Assert.Throws<OrderDomainException>(() => order.ChangeState(state));
    }

    [Fact]
    public void Customer_Cannot_Submit_Order_Without_Payment_And_Shipment()
    {
        var order = CreateOrder();

        Assert.Throws<OrderDomainException>(() => order.ChangeState(OrderState.Submitted));
    }
        
        
    #endregion
    
    [Fact]
    public void BuyerSubmittedOrderEvent_Is_Added_When_Order_Is_Submitted()
    {
        var order = CreateOrder();
        var shipmentAddress = new Address("street", "12345", "city", "country");
        order.ClearEvents();

        order.Submit(shipmentAddress);

        Assert.Single(order.DomainEvents);
        Assert.IsType<BuyerSubmittedOrderEvent>(order.DomainEvents.First());
    }
    
    [Fact]
    public void BuyerPaidOrderEvent_Is_Added_When_Order_Is_Paid()
    {
        var order = CreateSubmittedOrder();
        order.ClearEvents();
        
        order.ChangeState(OrderState.Paid);

        Assert.Single(order.DomainEvents);
        Assert.IsType<BuyerPaidOrderEvent>(order.DomainEvents.First());
    }
    
    [Fact]
    public void AdministratorShippedOrderEvent_Is_Added_When_Order_Is_Shipped()
    {
        var order = CreateSubmittedOrder();
        order.ChangeState(OrderState.Paid);
        order.ClearEvents();
        
        order.ChangeState(OrderState.Shipped);

        Assert.Single(order.DomainEvents);
        Assert.IsType<AdministratorShippedOrderEvent>(order.DomainEvents.First());
    }

    private Order CreateOrder()
    {
        return Order.Create(Guid.NewGuid());
    }
    
    private Order CreateSubmittedOrder()
    {
        var order = CreateOrder();
        var address = new Address("street", "12345", "city", "country");
        order.Submit(address);
        return order;
    }
}