using System.ComponentModel.DataAnnotations;

namespace ShoppingCart.Application.Dtos;

public class QuantityRequestDto
{
    [Required]
    public int Quantity { get; set; }
    
    public QuantityRequestDto(int quantity)
    {
        Quantity = quantity;
    }
}