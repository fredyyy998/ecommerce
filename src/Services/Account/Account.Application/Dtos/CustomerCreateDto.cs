using System.ComponentModel.DataAnnotations;

namespace Account.Application.Dtos;

public class CustomerCreateDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
    [Required]
    [MinLength(6)]
    public string Password { get; set; }
    
    public CustomerCreateDto(string email, string password)
    {
        Email = email;
        Password = password;
    }
}