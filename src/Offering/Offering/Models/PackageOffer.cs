namespace Offering.Models;

public class PackageOffer : Offer
{
    private List<Product> _products;
    
    public IReadOnlyCollection<Product> Products => _products.AsReadOnly();

    private PackageOffer() : base() {}
    protected PackageOffer(Guid id, string name, Price price, DateTime startDate, DateTime endDate, List<Product> products) : base( Guid.NewGuid(), name, price, startDate, endDate)
    {
        _products = products;
    }
    
    public static PackageOffer Create(string name, Price price, DateTime startDate, DateTime endDate, List<Product> products)
    {
        return new PackageOffer(Guid.NewGuid(), name, price, startDate, endDate, products);
    }
}