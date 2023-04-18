using ShoppingCart.Core.ShoppingCart;

namespace ShoppingCart.Test.ShoppingCart;

public class ShoppingCartCheckoutTest
{
    [Fact]
    public void Uses_ShippingAddress_As_BillingAddress_If_Not_Specified()
    {
        var shippingAddress = new Address("Street", "ZipCode", "City", "Country");
        
        var checkout = new ShoppingCartCheckout(Guid.NewGuid(), "FirstName", "LastName", "Email", shippingAddress, null);
        
        Assert.Equal(shippingAddress, checkout.BillingAddress);
    }
}