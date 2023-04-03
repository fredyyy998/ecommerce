using ShoppingCart.Core.Exceptions;
using ShoppingCart.Core.Product;

namespace ShoppingCart.Test.ProductTest;

public class ProductTest
{
    [Fact]
    public void Product_Stock_ShouldNotBeSmallerThanZero_WhenCreating()
    {
        var id = Guid.NewGuid();
        var name = "Test";
        var description = "Test";
        var price = new Price(10, 10, "EUR");
        var stock = -1;

        Assert.Throws<ProductDomainException>(() => Product.Create(id, name, description, price, stock));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Product_Stock_ShouldBeGreaterThanZero_WhenUpdating(int updatedStock)
    {
        var id = Guid.NewGuid();
        var name = "Test";
        var description = "Test";
        var price = new Price(10, 10, "EUR");
        var stock = 1;

        var product = Product.Create(id, name, description, price, stock);

        Assert.Throws<ProductDomainException>(() => product.Update(name, description, price, updatedStock));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Product_Stock_ShouldBeGreaterThanZero_WhenRemovingStock(int removeQuantity)
    {
        var id = Guid.NewGuid();
        var name = "Test";
        var description = "Test";
        var price = new Price(10, 10, "EUR");
        var stock = 1;

        var product = Product.Create(id, name, description, price, stock);

        Assert.Throws<ProductDomainException>(() => product.RemoveStock(removeQuantity));
    }
    
    [Fact]
    public void New_Product_Stock_ShouldNotBeSmallerThanZero_AfterRemovingStock()
    {
        var id = Guid.NewGuid();
        var name = "Test";
        var description = "Test";
        var price = new Price(10, 10, "EUR");
        var stock = 1;

        var product = Product.Create(id, name, description, price, stock);
        
        Assert.Throws<ProductDomainException>(() => product.RemoveStock(2));
    }
    
    [Fact]
    public void Product_Stock_Input_ShouldBeGreaterThanZero_WhenAddingStock()
    {
        var id = Guid.NewGuid();
        var name = "Test";
        var description = "Test";
        var price = new Price(10, 10, "EUR");
        var stock = 1;

        var product = Product.Create(id, name, description, price, stock);
        
        Assert.Throws<ProductDomainException>(() => product.AddStock(0));
    }
}