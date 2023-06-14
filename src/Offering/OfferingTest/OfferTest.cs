using Offering.Models;

namespace OfferingTest;

public class OfferTest
{
    [Fact]
    public void Add_Discount_To_Offer()
    {
        var price = Price.CreateFromGross(119, 19, "EUR");
        var product = Product.Create(Guid.NewGuid(), "test product", "test description");
        var offer = SingleOffer.Create("test offer", price, DateTime.Now, DateTime.MaxValue, product);
        
        var discount = Discount.Create(10, DateTime.Now, DateTime.MaxValue);
        offer.ApplyDiscount(discount);
        
        Assert.NotNull(offer.Discount);
        Assert.Equal(107.1m, offer.Price.GrossPrice);
        Assert.Equal(90m, offer.Price.NetPrice);
    }

    [Fact]
    public void Remove_Discount_From_Offer()
    {
        var price = Price.CreateFromGross(119, 19, "EUR");
        var product = Product.Create(Guid.NewGuid(), "test product", "test description");
        var offer = SingleOffer.Create("test offer", price, DateTime.Now, DateTime.MaxValue, product);
        var discount = Discount.Create(10, DateTime.Now, DateTime.MaxValue);
        offer.ApplyDiscount(discount);
        
        offer.RemoveDiscount();
        
        Assert.Null(offer.Discount);
        Assert.Equal(119, offer.Price.GrossPrice);
        Assert.Equal(100, offer.Price.NetPrice);
    }
    
}