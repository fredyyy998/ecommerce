using Ecommerce.Common.Core;
using Fulfillment.Core.Buyer;
using Fulfillment.Core.DomainEvents;
using Fulfillment.Core.Exceptions;

namespace Fulfillment.Core.Order;

public class Order : EntityRoot
{
    public Guid BuyerId { get; private set; }

    private List<OrderItem> _products;
    
    public IReadOnlyCollection<OrderItem> Products => _products.AsReadOnly();
    
    public Price TotalPrice { get; private set; }

    public Address ShipmentAddress { get; private set; }

    public DateTime OrderDate { get; private set; }
    
    public OrderState State { get; private set; }

    public static Order Create(Guid buyerId, Address shippingAddress)
    {
        return new Order()
        {
            Id = Guid.NewGuid(),
            BuyerId = buyerId,
            _products = new List<OrderItem>(),
            TotalPrice = new Price(0, 0, 19, "EUR"),
            State = OrderState.Submitted,
            OrderDate = DateTime.Now,
            ShipmentAddress = shippingAddress
        };
    }
    
    public void AddOrderItem(OrderItem orderItem)
    {
        _products.Add(orderItem);
        TotalPrice = new Price(
            TotalPrice.GrossPrice + orderItem.TotalPrice.GrossPrice,
            TotalPrice.NetPrice + orderItem.TotalPrice.NetPrice,
            TotalPrice.Tax,
            TotalPrice.Currency);
    }

    public void ChangeState(OrderState state)
    {
        if (state == OrderState.Cancelled && State != OrderState.Delivered)
        {
            State = state;
            AddDomainEvent(new OrderCancelledEvent(this));
        }
        else if (State == OrderState.Submitted && state == OrderState.Paid)
        {
            State = state;
            AddDomainEvent(new BuyerPaidOrderEvent(Id, BuyerId, DateTime.Now));
        } else if (State == OrderState.Paid && state == OrderState.Shipped)
        {
            State = state;
            AddDomainEvent(new AdministratorShippedOrderEvent(this));
        } else if (State == OrderState.Shipped && state == OrderState.Delivered)
        {
            State = state;
        } else
        {
            throw new OrderDomainException($"This state change from {State.ToString()} to {state.ToString()} is not allowed.");
        }
    }
}
