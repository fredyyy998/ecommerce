using Microsoft.EntityFrameworkCore;
using Offering.Models;

namespace Offering.Repositories;

public class LocalizationRepository : ILocalizationRepository
{
    private readonly DataContext _context;
    
    public LocalizationRepository(DataContext context)
    {
        _context = context;
    }
    
    public async Task<Localization> findByKey(string key)
    {
        return await _context.Localizations.FirstOrDefaultAsync(x => x.CountryCode == key);
    }
    
}