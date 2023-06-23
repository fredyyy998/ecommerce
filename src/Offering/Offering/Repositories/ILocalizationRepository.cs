using Offering.Models;

namespace Offering.Repositories;

public interface ILocalizationRepository
{
    Task<Localization> findByKey(string key);
}