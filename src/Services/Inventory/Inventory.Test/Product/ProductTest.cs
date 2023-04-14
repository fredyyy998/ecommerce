using Inventory.Core;
using Inventory.Core.Product;
using Xunit;

namespace Inventory.Test;

public class ProductTest
{
    [Fact]
    public void Product_is_created()
    {
        var product = Product.Create("Test Product", "Test Description", 100);
        
        Assert.Equal("Test Product", product.Name);
        Assert.Equal("Test Description", product.Description);
        Assert.Equal(100, product.Price.GrossPrice);
    }
    
    [Fact]
    public void Product_information_are_added()
    {
        var product = Product.Create("Test Product", "Test Description", 100);
        
        product.AddInformation("Test key", "test value");
        
        Assert.Equal("Test key", product.Informations.First().Key);
        Assert.Equal("test value", product.Informations.First().Value);
    }

    [Fact]
    public void Product_information_key_is_unique()
    {
        var product = Product.Create("Test Product", "Test Description", 100);
        product.AddInformation("Test key", "test value");
        
        Assert.Throws<InvalidOperationException>(() => product.AddInformation("Test key", "test value"));
        Assert.Equal(product.Informations.Count, 1);
    }

    [Fact]
    public void Product_information_is_removed()
    {
        var product = Product.Create("Test Product", "Test Description", 100);
        product.AddInformation("Test key", "test value");
        
        product.RemoveInformation("Test key");
        
        Assert.Empty(product.Informations);
    }
    
    [Fact]
    public void Product_base_information_are_updated()
    {
        var product = Product.Create("Test Product", "Test Description", 100);
        
        product.Update("New Name", "New Description", 200);
        
        Assert.Equal("New Name", product.Name);
    }
    
    [Fact]
    public void Product_stock_is_added()
    {
        var product = Product.Create("Test Product", "Test Description", 100);
        
        product.AddStock(10);
        
        Assert.Equal(10, product.Stock);
    }
    
    [Fact]
    public void Product_stock_add_value_must_be_positive()
    {
        var product = Product.Create("Test Product", "Test Description", 100);

        Assert.Throws<ProductDomainException>(() => product.AddStock(-10));
    }

    [Fact]
    public void Product_stock_is_reduced()
    {
        var product = Product.Create("Test Product", "Test Description", 100);
        product.AddStock(10);
        
        product.RemoveStock(5);
        
        Assert.Equal(5, product.Stock);
    }
    
    [Fact]
    public void Product_stock_remove_value_must_be_positive()
    {
        var product = Product.Create("Test Product", "Test Description", 100);

        Assert.Throws<ProductDomainException>(() => product.RemoveStock(-10));
    }
    
    [Fact]
    public void Product_stock_cannot_be_lower_than_zero()
    {
        var product = Product.Create("Test Product", "Test Description", 100);
        product.AddStock(5);

        Assert.Throws<ProductDomainException>(() => product.RemoveStock(10));
    }
}