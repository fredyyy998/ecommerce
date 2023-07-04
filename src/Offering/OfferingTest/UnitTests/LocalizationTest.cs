using Offering.Models;

namespace OfferingTest;

public class LocalizationTest
{
    [Fact]
    public void Localization_Is_Instantiated_Correctly()
    {
        var localization = Localization.Create("DE", "Germany", "Deutschland", "EUR");
        
        Assert.Equal("DE", localization.CountryCode);
        Assert.Equal("Germany", localization.CountryName);
        Assert.Equal("Deutschland", localization.LocalName);
        Assert.Equal("EUR", localization.Currency);
    }
}