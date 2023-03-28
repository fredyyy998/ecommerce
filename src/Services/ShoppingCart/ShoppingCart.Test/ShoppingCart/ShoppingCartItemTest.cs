using ShoppingCart.Core.Exceptions;
using ShoppingCart.Core.Product;
using ShoppingCart.Core.ShoppingCart;

namespace ShoppingCart.Test.ShoppingCart;

public class ShoppingCartItemTest
{
    [Fact]
    public void Quantity_ShouldBeGreaterThanZero()
    {
        var product = new Product(Guid.NewGuid(), "Test", "Test", new Price(10, 10, "EUR"), 10);
        var quantity = 0;
        
        Assert.Throws<ShoppingCartDomainException>(() => ShoppingCartItem.Create(product, quantity));
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Quantity_Increase_ShouldBeGreaterThanZero(int quantityIncrease)
    {
        var product = new Product(Guid.NewGuid(), "Test", "Test", new Price(10, 10, "EUR"), 10);
        var quantity = 1;
        var shoppingCartItem = ShoppingCartItem.Create(product, quantity);
        
        Assert.Throws<ShoppingCartDomainException>(() => shoppingCartItem.IncreaseQuantity(quantityIncrease));
    }

    [Fact]
    public void Quantity_Increase_Should_Calculate_New_Total_Price()
    {
        var product = new Product(Guid.NewGuid(), "Test", "Test", new Price(10, 10, "EUR"), 10);
        var quantity = 1;
        var shoppingCartItem = ShoppingCartItem.Create(product, quantity);
        
        shoppingCartItem.IncreaseQuantity(1);
        
        Assert.Equal(20, shoppingCartItem.TotalPrice);
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Quantity_Decrease_ShouldBeGreaterThanZero(int decreaseQuantity)
    {
        var product = new Product(Guid.NewGuid(), "Test", "Test", new Price(10, 10, "EUR"), 10);
        var quantity = 1;
        var shoppingCartItem = ShoppingCartItem.Create(product, quantity);
        
        Assert.Throws<ShoppingCartDomainException>(() => shoppingCartItem.DecreaseQuantity(decreaseQuantity));
    }
    
    [Fact]
    public void Quantity_Decrease_Should_Calculate_New_Total_Price()
    {
        var product = new Product(Guid.NewGuid(), "Test", "Test", new Price(10, 10, "EUR"), 10);
        var quantity = 5;
        var shoppingCartItem = ShoppingCartItem.Create(product, quantity);
        
        shoppingCartItem.DecreaseQuantity(1);
        
        Assert.Equal(40, shoppingCartItem.TotalPrice);
    }
}