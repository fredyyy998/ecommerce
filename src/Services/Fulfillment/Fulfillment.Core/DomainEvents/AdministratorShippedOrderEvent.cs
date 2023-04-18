using Ecommerce.Common.Core;

namespace Fulfillment.Core.DomainEvents;

public class AdministratorShippedOrderEvent : IDomainEvent
{
    public Guid OrderId { get; }

    public Guid BuyerId { get; }
    
    public DateTime ShippedAt { get; }
    
    public AdministratorShippedOrderEvent(Order.Order order)
    {
        OrderId = order.Id;
        BuyerId = order.BuyerId;
        ShippedAt = DateTime.UtcNow;
    }
}