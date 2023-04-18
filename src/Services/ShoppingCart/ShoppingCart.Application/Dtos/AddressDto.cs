namespace ShoppingCart.Application.Dtos;

public class AddressDto
{
    public string Street { get; private set; }
    
    public string ZipCode { get; private set; }
    
    public string City { get; private set; }
    
    public string Country { get; private set; }
    
    public AddressDto(string street, string zipCode, string city, string country)
    {
        Street = street;
        ZipCode = zipCode;
        City = city;
        Country = country;
    }
}