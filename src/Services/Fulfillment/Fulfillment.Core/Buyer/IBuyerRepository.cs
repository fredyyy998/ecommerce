namespace Fulfillment.Core.Buyer;

public interface IBuyerRepository
{
    Task<Buyer> FindByIdAsync(Guid buyerId);
    Task SaveAsync(Buyer buyer);
    Task UpdateAsync(Buyer buyer);
    Task DeleteAsync(Buyer buyer);
}