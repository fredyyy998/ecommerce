using System.ComponentModel.DataAnnotations;

namespace Account.Application.Dtos;

public class AddressDto
{
    [Required]
    public string Street { get; set; }
    
    [Required]
    public string City { get; set; }
    
    [Required]
    public string Zip { get; set; }
    
    [Required]
    public string Country { get; set; }
    
    public AddressDto(
        string street,
        string city,
        string zip,
        string country)
    {
        Street = street;
        City = city;
        Zip = zip;
        Country = country;
    }
}