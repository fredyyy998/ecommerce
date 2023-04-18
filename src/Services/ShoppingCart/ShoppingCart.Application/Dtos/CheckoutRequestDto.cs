using System.ComponentModel.DataAnnotations;

namespace ShoppingCart.Application.Dtos;

public class CheckoutRequestDto
{
    [Required]
    public Guid CustomerId { get; set; }
    
    [Required]
    public string FirstName { get; set; }
    
    [Required]
    public string LastName { get; set; }
    
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
    [Required]
    public AddressDto ShippingAddress { get; set; }
    
    public AddressDto? BillingAddress { get; set; }
}