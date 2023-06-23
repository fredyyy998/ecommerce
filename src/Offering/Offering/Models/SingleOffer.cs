namespace Offering.Models;

public class SingleOffer : Offer
{
    public Product Product { get; protected set; }
    
    private SingleOffer() : base() {}
    protected SingleOffer(Guid id, string name, Price price, DateTime startDate, DateTime endDate, Product product, Localization localization) : base( Guid.NewGuid(), name, price, startDate, endDate, localization)
    {
        Product = product;
    }

    public static SingleOffer Create(string name, Price price, DateTime startDate, DateTime endDate, Product product, Localization localization)
    {
        return new SingleOffer(Guid.NewGuid(), name, price, startDate, endDate, product, localization);
    }
}

