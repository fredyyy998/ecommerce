namespace Inventory.Core.Product;

public class Price
{
    public decimal GrossPrice { get; private set; }

    public decimal NetPrice { get; private set; }

    public static readonly int MaxDecimalPlaces = 2;
    
    public int SalesTax { get; private set; }
    
    public static readonly int DefaultSalesTax = 19;

    public string CurrencyCode { get; private set; }
    
    public static readonly string DefaultCurrencyCode = "EUR";
    
    public static Price CreateDefault(decimal GrossPrice)
    {
        return Price.Create(GrossPrice, DefaultSalesTax, DefaultCurrencyCode);
    }
    
    public static Price Create(decimal GrossPrice, int salesTax, string currencyCode)
    {
        return new Price
        {
            GrossPrice = GrossPrice,
            SalesTax = salesTax,
            CurrencyCode = currencyCode,
            NetPrice = CalculateNetPrice(GrossPrice, salesTax)
        };
    }

    public void UpdateGross(decimal grossPrice)
    {
        GrossPrice = grossPrice;
        NetPrice = CalculateNetPrice(grossPrice, SalesTax);
    }
    
    private static decimal CalculateNetPrice(decimal grossPrice, int salesTax)
    {
        return Math.Round(grossPrice / (100 + salesTax) * 100, MaxDecimalPlaces);
    }
}