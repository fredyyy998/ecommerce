using System.Data.Entity;
using ShoppingCart.Core.ShoppingCart;

namespace ShoppingCart.Infrastructure.Repositories;

public class ShoppingCartRepository : IShoppingCartRepository
{
    private readonly DataContext _context;
    
    public ShoppingCartRepository(DataContext context)
    {
        _context = context;
    }

    public Core.ShoppingCart.ShoppingCart GetById(Guid id)
    {
        return _context.ShoppingCarts.Find(id);
    }

    public void Create(Core.ShoppingCart.ShoppingCart entity)
    {
        _context.ShoppingCarts.Add(entity);
        _context.SaveChanges();
    }

    public void Update(Core.ShoppingCart.ShoppingCart entity)
    {
        _context.ShoppingCarts.Update(entity);
        _context.SaveChanges();
    }

    public void Delete(Guid id)
    {
        var shoppingCart = GetById(id);
        _context.ShoppingCarts.Remove(shoppingCart);
        _context.SaveChanges();
    }

    public List<Core.ShoppingCart.ShoppingCart> GetTimedOutShoppingCarts()
    {
        return _context.ShoppingCarts.Where(x => x.Status == State.TimedOut).ToList();
    }

    public List<Core.ShoppingCart.ShoppingCart> GetShoppingCartsByCustomerId(Guid customerId)
    {
        return _context.ShoppingCarts.Where(x => x.CustomerId == customerId).ToList();
    }

    public Core.ShoppingCart.ShoppingCart GetActiveShoppingCartByCustomer(Guid customerId)
    {
        // TODO there must be a better way to do this, but i really dont understand what is happening here
        // when the products are not loaded like this the shopping cart items are not populated correctly
        // somehow lazy loading is not working here
        _context.Products.ToList();
        var shoppingCart = _context.ShoppingCarts
            .Include(s => s.Items.Select(p =>p.Product))
            .FirstOrDefault(x => x.CustomerId == customerId && x.Status == State.Active);
        
        return shoppingCart;
    }
    
    public List<Core.ShoppingCart.ShoppingCart> GetActiveShoppingCartsCreatedBefore(DateTime date)
    {
        return _context.ShoppingCarts.Where(x => x.CreatedAt < date && x.Status == State.Active).ToList();
    }

    public void RemoveProductFromShoppingCart(Guid shoppingCartId, Guid productId)
    {
        var shoppingCart = GetById(shoppingCartId);
        var item = shoppingCart.Items.FirstOrDefault(x => x.Product.Id == productId);
        shoppingCart.RemoveQuantityOfItem(item.Product, item.Quantity);
        Update(shoppingCart);
    }
}