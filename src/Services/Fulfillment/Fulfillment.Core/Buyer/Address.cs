using Ecommerce.Common.Core;

namespace Fulfillment.Core.Buyer;

public class Address : ValueObject
{
    public string Street { get; private set; }
    
    public string ZipCode { get; private set; }
    
    public string City { get; private set; }
    
    public string Country { get; private set; }
    
    public Address(string street, string zipCode, string city, string country)
    {
        Street = street;
        ZipCode = zipCode;
        City = city;
        Country = country;
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Street;
        yield return ZipCode;
        yield return City;
        yield return Country;
    }
}