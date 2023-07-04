using Offering.Models;

namespace OfferingTest;

public class PriceTest
{
    [Fact]
    public void Create_Price_From_Gross_With_Net_Price()
    {
        var price = Price.CreateFromGross(119, 19);
        
        Assert.Equal(119, price.GrossPrice);
        Assert.Equal(100, price.NetPrice);
        Assert.Equal(19, price.TaxRate);
    }
    
    [Fact]
    public void Create_Price_With_Negative_Gross_throws_Exception()
    {
        Assert.Throws<ArgumentException>(() => Price.CreateFromGross(-119, 19));
    }
    
    [Fact]
    public void Create_Price_With_Negative_Tax_Rate_throws_Exception()
    {
        Assert.Throws<ArgumentException>(() => Price.CreateFromGross(119, -19));
    }
}