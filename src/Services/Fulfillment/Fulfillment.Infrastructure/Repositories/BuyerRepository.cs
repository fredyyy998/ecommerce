using Fulfillment.Core.Buyer;

namespace Fulfillment.Infrastructure.Repositories;

public class BuyerRepository : IBuyerRepository
{
    private DataContext _context;
    
    public BuyerRepository(DataContext context)
    {
        _context = context;
    }
    
    public Task<Buyer> FindByIdAsync(Guid buyerId)
    {
        return _context.Buyers.FindAsync(buyerId).AsTask();
    }

    public Task SaveAsync(Buyer buyer)
    {
        _context.Buyers.Add(buyer);
        return _context.SaveChangesAsync();
    }

    public Task UpdateAsync(Buyer buyer)
    {
        _context.Buyers.Update(buyer);
        return _context.SaveChangesAsync();
    }

    public Task DeleteAsync(Buyer buyer)
    {
        _context.Buyers.Remove(buyer);
        return _context.SaveChangesAsync();
    }
}