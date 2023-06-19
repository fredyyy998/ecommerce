package com.example.offersb.test;

import com.example.offersb.models.Price;
import org.junit.jupiter.api.Test;


public class PriceTest {

    @Test
    public void testCreateFromGross() {
        var price = Price.CreateFromGross(119, 19, "EUR");

        assert price.getGrossPrice() == 119;
        assert price.getNetPrice() == 100;
        assert price.getTaxRate() == 19;
        assert price.getCurrencyCode().equals("EUR");

    }
}
