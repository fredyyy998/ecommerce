namespace Offering.Models;

public class Price
{
    public decimal GrossPrice { get; private set; }
    
    public decimal NetPrice { get; private set; }
    
    public decimal TaxRate { get; private set; }

    private Price(decimal grossPrice, decimal netPrice, decimal taxRate)
    {
        GrossPrice = grossPrice;
        NetPrice = netPrice;
        TaxRate = taxRate;
    }
    
    public static Price CreateFromGross(decimal grossPrice, decimal taxRate)
    {
        if (grossPrice < 0)
        {
            throw new ArgumentException("Gross price cannot be negative");
        }

        if (taxRate < 0)
        {
            throw new ArgumentException("Tax rate cannot be negative");
        }

        var netPrice = grossPrice / (1 + taxRate / 100);
        return new Price(grossPrice, netPrice, taxRate);
    }
}