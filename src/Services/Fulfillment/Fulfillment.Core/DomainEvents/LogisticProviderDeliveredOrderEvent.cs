using Ecommerce.Common.Core;

namespace Fulfillment.Core.DomainEvents;

public class LogisticProviderDeliveredOrderEvent : IDomainEvent
{
    Guid OrderId { get; }
    
    Guid BuyerId { get; }
    
    DateTime DeliveredAt { get; }
    
    public LogisticProviderDeliveredOrderEvent(Order.Order order)
    {
        OrderId = order.Id;
        BuyerId = order.BuyerId;
        DeliveredAt = DateTime.UtcNow;
    }
}