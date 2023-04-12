using Ecommerce.Common.Core;

namespace Fulfillment.Core.DomainEvents;

public class LogisticProviderDeliveredOrderEvent : IDomainEvent
{
    public Guid OrderId { get; }
    
    public Guid BuyerId { get; }
    
    public DateTime DeliveredAt { get; }
    
    public LogisticProviderDeliveredOrderEvent(Order.Order order)
    {
        OrderId = order.Id;
        BuyerId = order.BuyerId;
        DeliveredAt = DateTime.UtcNow;
    }
}