using Inventory.Core.Product;
using Xunit;

namespace Inventory.Test;

public class PriceTest
{
    [Fact]
    public void Price_is_created()
    {
        var price = Price.Create(119, 19, "EUR");
        
        Assert.Equal(119, price.GrossPrice);
        Assert.Equal(100, price.NetPrice);
        Assert.Equal(19, price.SalesTax);
        Assert.Equal("EUR", price.CurrencyCode);
    }
    
    [Fact]
    public void Default_price_is_created()
    {
        var price = Price.CreateDefault(119);
        
        Assert.Equal(119, price.GrossPrice);
        Assert.Equal(100, price.NetPrice);
        Assert.Equal(19, price.SalesTax);
        Assert.Equal("EUR", price.CurrencyCode);
    }

    [Fact]
    public void Prices_are_updated()
    {
        var price = Price.CreateDefault(119);
        
        price.UpdateGross(100);
        
        Assert.Equal(100, price.GrossPrice);
        Assert.Equal(84.03m, price.NetPrice);
    }
}