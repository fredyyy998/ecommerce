using Ecommerce.Common.Core;

namespace Fulfillment.Core.DomainEvents;

public class BuyerPaidOrderEvent : IDomainEvent
{
    public Guid OrderId { get; }
    public Guid BuyerId { get; }
    public DateTime PaidAt { get; }

    public BuyerPaidOrderEvent(Guid orderId, Guid buyerId, DateTime paidAt)
    {
        OrderId = orderId;
        BuyerId = buyerId;
        PaidAt = paidAt;
    }
}