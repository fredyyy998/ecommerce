using Fulfillment.Core.Buyer;

namespace Fulfillment.Core.DomainEvents;

public record ShoppingCartCheckoutDto(
    Guid CustomerId,
    string FirstName,
    string LastName,
    string Email,
    Address ShippingAddress,
    Address BillingAddress);