using Ecommerce.Common.Core;

namespace Fulfillment.Core.DomainEvents;

public class AdministratorShippedOrderEvent : IDomainEvent
{
    Guid OrderId { get; }

    Guid BuyerId { get; }
    
    DateTime ShippedAt { get; }
    
    public AdministratorShippedOrderEvent(Order.Order order)
    {
        OrderId = order.Id;
        BuyerId = order.BuyerId;
        ShippedAt = DateTime.UtcNow;
    }
}