namespace Offering.Models;

public class Discount
{
    public decimal DiscountRate { get; private set; }
    
    public DateTime StartDate { get; private set; }
    
    public DateTime EndDate { get; private set; }
    
    protected Discount(decimal discountRate, DateTime startDate, DateTime endDate)
    {
        DiscountRate = discountRate;
        StartDate = startDate;
        EndDate = endDate;
    }
    
    public static Discount Create(decimal discountRate, DateTime startDate, DateTime endDate)
    {
        return new Discount(discountRate, startDate, endDate);
    }
}