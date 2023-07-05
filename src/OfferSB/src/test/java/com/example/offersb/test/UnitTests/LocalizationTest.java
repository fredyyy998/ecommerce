package com.example.offersb.test.UnitTests;

import com.example.offersb.models.Localization;
import org.junit.jupiter.api.Test;

public class LocalizationTest {

    @Test
    public void testLocalization_Is_Instantiated_Correctly() {
        var localization = Localization.create("DE", "Germany", "Deutschland", "EUR");

        assert localization.getCountryCode() == "DE";
        assert localization.getCountryName() == "Germany";
        assert localization.getLocalName() == "Deutschland";
        assert localization.getCurrency() == "EUR";
    }
}
