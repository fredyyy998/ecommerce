using Ecommerce.Common.Core;

namespace Fulfillment.Core.Order;

public class Price : ValueObject
{
    public decimal GrossPrice { get; private set; }
    
    public decimal NetPrice { get; private set; }
    
    public decimal Tax { get; private set; }
    
    public string Currency { get; private set; }
    
    public Price(decimal grossPrice, decimal netPrice, decimal tax, string currency)
    {
        GrossPrice = grossPrice;
        NetPrice = netPrice;
        Tax = tax;
        Currency = currency;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return GrossPrice;
        yield return NetPrice;
        yield return Tax;
    }
}