using ShoppingCart.Core.Exceptions;
using ShoppingCart.Core.Product;

namespace ShoppingCart.Test.ProductTest;

public class ProductTest
{
    [Fact]
    public void Product_Stock_ShouldBeGreaterThanZero()
    {
        var id = Guid.NewGuid();
        var name = "Test";
        var description = "Test";
        var price = new Price(10, 10, "EUR");
        var stock = 0;
        
        Assert.Throws<ProductDomainException>(() => Product.Create(id, name, description, price, stock));
    }
}