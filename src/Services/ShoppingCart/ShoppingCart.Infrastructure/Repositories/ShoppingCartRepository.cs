using Microsoft.EntityFrameworkCore;
using Ecommerce.Common.Core;
using ShoppingCart.Core.ShoppingCart;

namespace ShoppingCart.Infrastructure.Repositories;

public class ShoppingCartRepository : IShoppingCartRepository
{
    private readonly DataContext _context;
    
    public ShoppingCartRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<Core.ShoppingCart.ShoppingCart> GetById(Guid id)
    {
        return await _context.ShoppingCarts.FindAsync(id);
    }

    public async Task<Core.ShoppingCart.ShoppingCart> Create(Core.ShoppingCart.ShoppingCart entity)
    {
        _context.ShoppingCarts.Add(entity);
        await _context.SaveChangesAsync();
        return await GetById(entity.Id);
    }

    public async Task Update(Core.ShoppingCart.ShoppingCart entity)
    {
        _context.ShoppingCarts.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(Guid id)
    {
        var shoppingCart = await GetById(id);
        _context.ShoppingCarts.Remove(shoppingCart);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Core.ShoppingCart.ShoppingCart>> GetTimedOutShoppingCarts()
    {
        return await _context.ShoppingCarts.Where(x => x.Status == State.TimedOut).ToListAsync();
    }

    public async Task<List<Core.ShoppingCart.ShoppingCart>> GetShoppingCartsByCustomerId(Guid customerId)
    {
        return await _context.ShoppingCarts.Where(x => x.CustomerId == customerId).ToListAsync();
    }

    public async Task<Core.ShoppingCart.ShoppingCart> GetActiveShoppingCartByCustomer(Guid customerId)
    {
        // TODO there must be a better way to do this, but i really dont understand what is happening here
        // when the products are not loaded like this the shopping cart items are not populated correctly
        // somehow lazy loading is not working here
        await _context.Products.ToListAsync();
        var shoppingCart = await _context.ShoppingCarts
            .Include(s => s.Items)
            .FirstOrDefaultAsync(x => x.CustomerId == customerId && x.Status == State.Active);
        
        return shoppingCart;
    }
    
    
    public async Task<List<Core.ShoppingCart.ShoppingCart>> GetActiveShoppingCartsCreatedBefore(DateTime date)
    {
        return await _context.ShoppingCarts.Where(x => x.CreatedAt < date && x.Status == State.Active).ToListAsync();
    }

    public async Task RemoveProductFromShoppingCart(Guid shoppingCartId, Guid productId)
    {
        var shoppingCart = await GetById(shoppingCartId);
        var item = shoppingCart.Items.FirstOrDefault(x => x.Product.Id == productId);
        shoppingCart.RemoveQuantityOfItem(item.Product, item.Quantity);
        Update(shoppingCart);
    }

    public Task<PagedList<Core.ShoppingCart.ShoppingCart>> FindAll(PaginationParameter paginationParameter)
    {
        throw new NotImplementedException();
    }
}