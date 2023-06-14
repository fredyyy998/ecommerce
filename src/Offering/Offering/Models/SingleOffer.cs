namespace Offering.Models;

public class SingleOffer : Offer
{
    public Product Product { get; protected set; }
    
    protected SingleOffer(Guid id, string name, Price price, DateTime startDate, DateTime endDate, Product product) : base( Guid.NewGuid(), name, price, startDate, endDate)
    {
        Product = product;
    }

    public static SingleOffer Create(string name, Price price, DateTime startDate, DateTime endDate, Product product)
    {
        return new SingleOffer(Guid.NewGuid(), name, price, startDate, endDate, product);
    }
}

