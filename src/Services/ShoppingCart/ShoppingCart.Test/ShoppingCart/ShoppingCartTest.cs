using ShoppingCart.Core.Exceptions;
using ShoppingCart.Core.Product;

namespace ShoppingCart.Test.ShoppingCart;

public class ShoppingCartTest
{
    [Fact]
    public void New_Added_Item_Should_Create_New_ShoppingCartItem()
    {
        var shoppingCart = Core.ShoppingCart.ShoppingCart.Create(Guid.NewGuid());
        var product = GetProduct();
        var quantity = 1;
        
        shoppingCart.AddItem(product, quantity);
        
        Assert.Single(shoppingCart.Items);
        Assert.Equal(product.Id, shoppingCart.Items.First().Product.Id);
    }
    
    [Fact]
    public void Existing_Item_Should_Increase_Quantity()
    {
        var shoppingCart = Core.ShoppingCart.ShoppingCart.Create(Guid.NewGuid());
        var product = GetProduct();
        var quantity = 1;
        
        shoppingCart.AddItem(product, quantity);
        shoppingCart.AddItem(product, quantity);
        
        Assert.Single(shoppingCart.Items);
        Assert.Equal(2, shoppingCart.Items.First().Quantity);
    }
    
    [Fact]
    public void Existing_Item_Should_Decrease_Quantity()
    {
        var shoppingCart = Core.ShoppingCart.ShoppingCart.Create(Guid.NewGuid());
        var product = GetProduct();
        shoppingCart.AddItem(product, 5);
        
        shoppingCart.RemoveQuantityOfProduct(product, 1);
        
        Assert.Single(shoppingCart.Items);
        Assert.Equal(4, shoppingCart.Items.First().Quantity);
    }
    
    [Fact]
    public void Existing_Item_Should_Remove_When_Quantity_Is_Zero()
    {
        var shoppingCart = Core.ShoppingCart.ShoppingCart.Create(Guid.NewGuid());
        var product = GetProduct();
        shoppingCart.AddItem(product, 5);
        
        shoppingCart.RemoveQuantityOfProduct(product, 5);
        
        Assert.Empty(shoppingCart.Items);
    }
    
    [Fact]
    public void NotExisting_Item_Should_Throw_Exception()
    {
        var shoppingCart = Core.ShoppingCart.ShoppingCart.Create(Guid.NewGuid());
        var product = GetProduct(); 
        
        Assert.Throws<ShoppingCartDomainException>(() => shoppingCart.RemoveQuantityOfProduct(product, 1));
    }

    public Product GetProduct()
    {
        return Product.Create(Guid.NewGuid(), "Test", "Test", new Price(10, 10, "EUR"), 10);
    }
}