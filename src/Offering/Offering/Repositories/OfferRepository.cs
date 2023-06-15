using Microsoft.EntityFrameworkCore;
using Offering.Models;

namespace Offering.Repositories;

public class OfferRepository : IOfferRepository
{
    
    public DataContext _context { get; set; }
    
    public OfferRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<List<Offer>> FindAll()
    {
        return await _context.Offers.ToListAsync();
    }

    public async Task<List<Offer>> FindAllAvailable()
    {
        return await _context.Offers.Where(o => o.EndDate > DateTime.Now).ToListAsync();
    }

    public async Task<List<Offer>> FindAllExpired()
    {
        return await _context.Offers.Where(o => o.EndDate < DateTime.Now).ToListAsync();
    }

    public async Task<Offer> FindById(Guid id)
    {
        return await _context.Offers.FindAsync(id);
    }

    public async Task<List<Offer>> findByProduct(Guid productId)
    {
        var singleOffers = await _context.Offers.OfType<SingleOffer>().Where(o => o.Product.Id == productId).ToListAsync();
        var packageOffers = await _context.Offers.OfType<PackageOffer>().Where(o => o.Products.Any(p => p.Id == productId)).ToListAsync();
        return singleOffers.Concat<Offer>(packageOffers).ToList();
    }

    public async Task<Offer> Add(Offer offer)
    {
        _context.Offers.Add(offer);
        await _context.SaveChangesAsync();
        return await FindById(offer.Id);
    }

    public async Task Update(Offer offer)
    {
        _context.Offers.Update(offer);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(Offer offer)
    {
        _context.Offers.Remove(offer);
        await _context.SaveChangesAsync();
    }
}