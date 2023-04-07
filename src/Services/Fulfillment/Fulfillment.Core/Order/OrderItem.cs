using Ecommerce.Common.Core;

namespace Fulfillment.Core.Order;

public class OrderItem : ValueObject
{
    public string ProductId { get; private set; }
    public string Name { get; private set; }
    public Price Price { get; private set; }
    public int Quantity { get; private set; }
    public Price TotalPrice { get; private set; }
    
    public OrderItem(string name, Price price, int quantity)
    {
        Name = name;
        Price = price;
        Quantity = quantity;
        TotalPrice = new Price(
            price.GrossPrice * quantity,
            price.NetPrice * quantity,
            price.Tax,
            price.Currency);
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