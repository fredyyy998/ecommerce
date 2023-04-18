using System.ComponentModel.DataAnnotations;

namespace Inventory.Application.Dtos;

public class QuantityDto
{
    [Required]
    public int Quantity { get; set; }
    
    public QuantityDto(int quantity)
    {
        Quantity = quantity;
    }
}