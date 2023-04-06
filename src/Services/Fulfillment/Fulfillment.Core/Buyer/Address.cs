using Ecommerce.Common.Core;

namespace Fulfillment.Core.Buyer;

public class Address : ValueObject
{
    public string Street { get; private set; }
    
    public string Zip { get; private set; }
    
    public string City { get; private set; }
    
    public string Country { get; private set; }
    
    public Address(string street, string zip, string city, string country)
    {
        Street = street;
        Zip = zip;
        City = city;
        Country = country;
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Street;
        yield return Zip;
        yield return City;
        yield return Country;
    }
}