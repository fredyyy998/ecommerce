namespace Offering.Models;

public class Price
{
    public decimal GrossPrice { get; private set; }
    
    public decimal NetPrice { get; private set; }
    
    public decimal TaxRate { get; private set; }
    
    public string CurrencyCode { get; private set; }

    private Price(decimal grossPrice, decimal netPrice, decimal taxRate, string currencyCode)
    {
        GrossPrice = grossPrice;
        NetPrice = netPrice;
        TaxRate = taxRate;
        CurrencyCode = currencyCode;
    }
    
    public static Price CreateFromGross(decimal grossPrice, decimal taxRate, string currencyCode)
    {
        if (grossPrice < 0)
        {
            throw new ArgumentException("Gross price cannot be negative");
        }

        if (taxRate < 0)
        {
            throw new ArgumentException("Tax rate cannot be negative");
        }
        
        if (string.IsNullOrWhiteSpace(currencyCode))
        {
            throw new ArgumentException("Currency code cannot be empty");
        }
        
        var netPrice = grossPrice / (1 + taxRate / 100);
        return new Price(grossPrice, netPrice, taxRate, currencyCode);
    }
}