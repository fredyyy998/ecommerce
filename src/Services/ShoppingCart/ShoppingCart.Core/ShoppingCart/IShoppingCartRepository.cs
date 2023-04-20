using System.Collections.ObjectModel;
using Ecommerce.Common.Core;

namespace ShoppingCart.Core.ShoppingCart;

public interface IShoppingCartRepository : IRepository<ShoppingCart>
{
    Task<List<ShoppingCart>> GetTimedOutShoppingCarts();
    Task<List<ShoppingCart>> GetShoppingCartsByCustomerId(Guid customerId);
    Task<ShoppingCart> GetActiveShoppingCartByCustomer(Guid customerId);
    Task<List<ShoppingCart>> GetActiveShoppingCartsCreatedBefore(DateTime date);
    Task RemoveProductFromShoppingCart(Guid shoppingCartId, Guid productId);
}