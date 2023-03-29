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

    [Fact]
    public void Product_Stock_Should_Decrease_When_Added_To_ShoppingCart()
    {
        var shoppingCart = Core.ShoppingCart.ShoppingCart.Create(Guid.NewGuid());
        var product = GetProduct(2);
        var quantity = 3;
        
        
        Assert.Throws<ShoppingCartDomainException>(() => shoppingCart.AddItem(product, quantity));
        Assert.Equal(2, product.Stock);
        Assert.Empty(shoppingCart.Items);
    }
    
    [Fact]
    public void Product_Cannot_Be_Added_When_Stock_ProdcutStock_Is_Not_Sufficent()
    {
        var shoppingCart = Core.ShoppingCart.ShoppingCart.Create(Guid.NewGuid());
        var product = GetProduct();
        var quantity = 10;
    }
    
    [Fact]
    public void Product_Stock_Should_Increase_When_Removed_From_ShoppingCart()
    {
        var shoppingCart = Core.ShoppingCart.ShoppingCart.Create(Guid.NewGuid());
        var product = GetProduct();
        var quantity = 1;
        shoppingCart.AddItem(product, quantity);
        
        shoppingCart.RemoveQuantityOfProduct(product, quantity);
        
        Assert.Equal(10, product.Stock);
    }
    
    public Product GetProduct(int stock = 10)
    {
        return Product.Create(Guid.NewGuid(), "Test", "Test", new Price(10, 10, "EUR"), stock);
    }
}