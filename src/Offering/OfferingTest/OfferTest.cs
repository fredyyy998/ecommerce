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

    [Fact]
    public void Single_Offer_Is_Instantiated_Correctly()
    {
        var price = Price.CreateFromGross(119, 19, "EUR");
        var product = Product.Create(Guid.NewGuid(), "test product", "test description");
        var dateStart = DateTime.Now;
        var dateEnd = DateTime.MaxValue;
        
        var offer = SingleOffer.Create("test offer", price, dateStart, dateEnd, product);
        
        Assert.Equal("test offer", offer.Name);
        Assert.Equal(price, offer.Price);
        Assert.Equal(dateStart, offer.StartDate);
        Assert.Equal(dateEnd, offer.EndDate);
        Assert.Equal(product, offer.Product);
        Assert.Null(offer.Discount);
    }

    [Fact]
    public void Package_Offer_Is_Instantiated_Correctly()
    {
        var price = Price.CreateFromGross(119, 19, "EUR");
        var product1 = Product.Create(Guid.NewGuid(), "test product", "test description");
        var product2 = Product.Create(Guid.NewGuid(), "test product2", "test description");
        var products = new List<Product> {product1, product2};
        var dateStart = DateTime.Now;
        var dateEnd = DateTime.MaxValue;
        var offer = PackageOffer.Create("test offer", price, dateStart, dateEnd, products);
        
        Assert.Equal("test offer", offer.Name);
        Assert.Equal(price, offer.Price);
        Assert.Equal(dateStart, offer.StartDate);
        Assert.Equal(dateEnd, offer.EndDate);
        Assert.Equal(products, offer.Products);
        Assert.Null(offer.Discount);
    }

    [Fact]
    public void Cannot_Apply_2_Discounts_To_Offer()
    {
        var price = Price.CreateFromGross(119, 19, "EUR");
        var product = Product.Create(Guid.NewGuid(), "test product", "test description");
        var offer = SingleOffer.Create("test offer", price, DateTime.Now, DateTime.MaxValue, product);
        var discount1 = Discount.Create(10, DateTime.Now, DateTime.MaxValue);
        offer.ApplyDiscount(discount1);
        
        var discount2 = Discount.Create(20, DateTime.Now, DateTime.MaxValue);
        
        Assert.Throws<InvalidOperationException>(() => offer.ApplyDiscount(discount2));
    }
    
}