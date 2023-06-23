namespace Offering.Models;

public abstract class Offer
{
    public Guid Id { get; protected set; }
    
    public string Name { get; protected set; }
    
    public Price Price { get; protected set; }
    
    public Discount? Discount { get; protected set; }
    
    public DateTime StartDate { get; protected set; }
    
    public DateTime EndDate { get; protected set; }
    
    public Localization Localization { get; protected set; }

    protected Offer() {}
    protected Offer(Guid id, string name, Price price, DateTime startDate, DateTime endDate, Localization localization)
    {
        Id = id;
        Name = name;
        Price = price;
        Discount = null;
        StartDate = startDate;
        EndDate = endDate;
        Localization = localization;
    }
    
    public void ApplyDiscount(Discount discount)
    {
        if (Discount != null)
        {
            throw new InvalidOperationException("Discount already exists");
        }
        Discount = discount;
        Price = CalculatePriceFromDiscount(discount, Price);
    }
    
    public void RemoveDiscount()
    {
        Price = RemoveDiscountFromPrice(Discount, Price);
        Discount = null;
    }

    private Price CalculatePriceFromDiscount(Discount discount, Price price)
    {
        var discountPrice = discount.DiscountRate / 100 * price.GrossPrice;
        return Price.CreateFromGross(price.GrossPrice - discountPrice, price.TaxRate);
    }
    
    private Price RemoveDiscountFromPrice(Discount discount, Price price)
    {
        var discountPrice = discount.DiscountRate / (100 - discount.DiscountRate) * price.GrossPrice;
        return Price.CreateFromGross(price.GrossPrice + discountPrice, price.TaxRate);
    }
}



