using Ecommerce.Common.Core;

namespace ShoppingCart.Core.Product;

public class Price : ValueObject
{
    public decimal NetPrice { get; private set; }
    
    public decimal GrossPrice { get; private set; }
    
    public string CurrencyCode { get; private set; }
    
    
    public Price(decimal netPrice, decimal grossPrice, string currencyCode)
    {
        NetPrice = netPrice;
        GrossPrice = grossPrice;
        CurrencyCode = currencyCode;
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return NetPrice;
        yield return GrossPrice;
        yield return CurrencyCode;
    }
}