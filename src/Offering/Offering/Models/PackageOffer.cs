namespace Offering.Models;

public class PackageOffer : Offer
{
    private List<Product> _products;
    
    public IReadOnlyCollection<Product> Products => _products.AsReadOnly();

    private PackageOffer() : base() {}
    protected PackageOffer(Guid id, string name, Price price, DateTime startDate, DateTime endDate, List<Product> products, Localization localization) : base(id, name, price, startDate, endDate, localization)
    {
        _products = products;
    }
    
    public static PackageOffer Create(string name, Price price, DateTime startDate, DateTime endDate, List<Product> products, Localization localization)
    {
        return new PackageOffer(Guid.NewGuid(), name, price, startDate, endDate, products, localization);
    }
    
    public static PackageOffer Create(Guid id, string name, Price price, DateTime startDate, DateTime endDate, List<Product> products, Localization localization)
    {
        return new PackageOffer(id, name, price, startDate, endDate, products, localization);
    }
}