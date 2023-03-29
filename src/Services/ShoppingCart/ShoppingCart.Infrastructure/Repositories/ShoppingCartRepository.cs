using System.Collections.ObjectModel;
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
        return _context.ShoppingCarts.FirstOrDefault(x => x.CustomerId == customerId && x.Status == State.Active);
    }
}