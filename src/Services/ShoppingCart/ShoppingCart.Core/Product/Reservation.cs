using Ecommerce.Common.Core;

namespace ShoppingCart.Core.Product;

public class Reservation : ValueObject
{
    public Guid ShoppingCartId { get; private set; }
    
    public int Quantity { get; private set; }
    
    public DateTime CreatedAt { get; private set; }
    
    public Reservation(Guid shoppingCartId, int quantity)
    {
        ShoppingCartId = shoppingCartId;
        Quantity = quantity;
        CreatedAt = DateTime.UtcNow;
    }
    
    public static Reservation Create(Guid shoppingCartId, int quantity)
    {
        return new Reservation(shoppingCartId, quantity);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return ShoppingCartId;
        yield return Quantity;
        yield return CreatedAt;
    }
}