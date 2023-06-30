namespace Offering.Models;

public class Localization
{
    public string CountryCode { get; private set; }
    
    public string CountryName { get; private set; }
    
    public string LocalName { get; private set; }
    
    public string Currency { get; private set; }
    
    protected Localization(string countryCode, string countryName, string localName, string currency)
    {
        CountryCode = countryCode;
        CountryName = countryName;
        LocalName = localName;
        Currency = currency;
    }
    
    public static Localization Create(string countryCode, string countryName, string localName, string currency)
    {
        return new Localization(countryCode, countryName, localName, currency);
    }
}