namespace Inventory.Core.Product;

public class ProductInformation
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
}