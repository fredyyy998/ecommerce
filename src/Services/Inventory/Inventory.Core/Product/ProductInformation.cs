using Ecommerce.Common.Core;

namespace Inventory.Core.Product;

public class ProductInformation : ValueObject
{
    public string Key { get; private set; }
    public string Value { get; private set; }
    
    public static ProductInformation Create(string key, string value)
    {
        return new ProductInformation
        {
            Key = key,
            Value = value
        };
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Key;
        yield return Value;
    }
}