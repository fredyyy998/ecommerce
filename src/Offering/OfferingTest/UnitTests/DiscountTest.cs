using Offering.Models;

namespace OfferingTest;

public class DiscountTest
{
    [Fact]
    public void Discount_Is_Instantiated_Correctly()
    {
        var dateStart = DateTime.Now;
        var dateEnd = DateTime.MaxValue;
        var discount = Discount.Create(10, dateStart, dateEnd);
        
        Assert.Equal(10, discount.DiscountRate);
        Assert.Equal(dateStart, discount.StartDate);
        Assert.Equal(dateEnd, discount.EndDate);
    }
}