using Offering.Models;

namespace Offering.Repositories;

public interface IOfferRepository
{
    Task<List<Offer>> FindAll(); 
    
    Task<List<Offer>> FindAllAvailable();

    Task<List<Offer>> FindAllExpired();
    
    Task<Offer> FindById(Guid id);
    
    Task<List<Offer>> findByProduct(Guid productId);
    
    Task<Offer> Add(Offer offer);
    
    Task Update(Offer offer);
    
    Task Delete(Offer offer);
}