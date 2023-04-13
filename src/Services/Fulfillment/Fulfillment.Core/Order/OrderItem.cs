using Ecommerce.Common.Core;

namespace Fulfillment.Core.Order;

public class OrderItem : ValueObject
{
    public string ProductId { get; private set; }
    public string Name { get; private set; }
    public Price Price { get; private set; }
    public int Quantity { get; private set; }
    public Price TotalPrice { get; private set; }
    
    public static OrderItem Create(Guid productId, string name, decimal grossPrice, decimal netPrice, string currencyCode, decimal tax, int quantity)
    {
        return new OrderItem
        {
            ProductId = productId.ToString(),
            Name = name,
            Price = new Price(grossPrice, netPrice, tax, currencyCode),
            Quantity = quantity,
            TotalPrice = new Price(
                grossPrice * quantity,
                netPrice * quantity,
                tax,
                currencyCode)
        };
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return ProductId;
        yield return Name;
        yield return Price;
        yield return Quantity;
        yield return TotalPrice;
    }
}