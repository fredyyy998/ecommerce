namespace Fulfillment.Core.Order;

public enum OrderState
{
    Created,
    Submitted,
    Paid,
    Shipped,
    Delivered,
    Cancelled
}