using System.ComponentModel.DataAnnotations;

namespace Fulfillment.Application.Dtos;

public class RevenueQuery
{
    [Required]
    public DateTime StartDate { get; set; }
    
    [Required]
    public DateTime EndDate { get; set; }
}