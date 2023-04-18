using Ecommerce.Common.Core;

namespace ShoppingCart.Core.ShoppingCart;

public class ShoppingCartCheckout : ValueObject
{
    public Guid CustomerId { get; private set; }
    
    public string FirstName { get; private set; }
    
    public string LastName { get; private set; }

    public string Email { get; private set; }
    
    public Address ShippingAddress { get; private set; }
    
    public Address BillingAddress { get; private set; }
    
    public ShoppingCartCheckout(Guid customerId, string firstName, string lastName, string email, Address shippingAddress, Address? billingAddress)
    {
        CustomerId = customerId;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        ShippingAddress = shippingAddress;
        if (billingAddress == null)
        {
            billingAddress = shippingAddress;
        }
        BillingAddress = billingAddress;
    }
    
    public ShoppingCartCheckout(Guid customerId, string firstName, string lastName, string email, Address shippingAddress)
        : this(customerId, firstName, lastName, email, shippingAddress, null)
    {
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return CustomerId;
        yield return FirstName;
        yield return LastName;
        yield return Email;
        yield return ShippingAddress;
        yield return BillingAddress;
    }
}