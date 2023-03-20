using Ecommerce.Common.Core;

namespace Account.Core.User;

public class Address : ValueObject
{
    public string Street { get; protected set; }
    public string City { get; protected set; }
    public string Zip { get; protected set; }
    public string Country { get; protected set; }

    protected Address() { }

    public Address(string street, string zip, string city, string country)
    {
        Street = street;
        City = city;
        Zip = zip;
        Country = country;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Street;
        yield return City;
        yield return Country;
        yield return Zip;
    }
}