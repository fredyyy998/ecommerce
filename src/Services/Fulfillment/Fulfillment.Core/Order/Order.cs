using Ecommerce.Common.Core;
using Fulfillment.Core.Exceptions;

namespace Fulfillment.Core.Order;

public class Order : EntityRoot
{
    public Buyer.Buyer Buyer { get; private set; }

    private List<OrderItem> _products;
    
    public IReadOnlyCollection<OrderItem> Products => _products.AsReadOnly();
    
    public Price TotalPrice { get; private set; }

    public DateTime OrderDate { get; private set; }
    
    public OrderState State { get; private set; }

    public static Order Create(Buyer.Buyer buyer)
    {
        return new Order()
        {
            Id = Guid.NewGuid(),
            Buyer = buyer,
            _products = new List<OrderItem>(),
            TotalPrice = new Price(0, 0, 19, "EUR"),
            State = OrderState.Created,
            OrderDate = DateTime.Now
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
        } else if (State == OrderState.Created && state == OrderState.Submitted)
        {
            State = state;
        } else if (State == OrderState.Submitted && state == OrderState.Paid)
        {
            State = state;
        } else if (State == OrderState.Paid && state == OrderState.Shipped)
        {
            State = state;
        } else if (State == OrderState.Shipped && state == OrderState.Delivered)
        {
            State = state;
        } else
        {
            throw new OrderDomainException($"This state change from {State.ToString()} to {state.ToString()} is not allowed.");
        }
    }
}
