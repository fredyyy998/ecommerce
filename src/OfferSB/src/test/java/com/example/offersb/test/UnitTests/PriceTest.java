package com.example.offersb.test.UnitTests;

import com.example.offersb.models.Price;
import org.junit.jupiter.api.Test;

import static org.junit.jupiter.api.Assertions.assertThrows;


public class PriceTest {

    @Test
    public void testCreateFromGross() {
        var price = Price.CreateFromGross(119, 19);

        assert price.getGrossPrice() == 119;
        assert price.getNetPrice() == 100;
        assert price.getTaxRate() == 19;
    }

    @Test
    public void testCreate_Price_With_Negative_Gross_throws_Exception() {
        assertThrows(IllegalArgumentException.class, () -> {
            Price.CreateFromGross(-119, 19);
        });
    }

    @Test
    public void testCreate_Price_With_Negative_Tax_Rate_throws_Exception() {
        assertThrows(IllegalArgumentException.class, () -> {
            Price.CreateFromGross(119, -19);
        });
    }
}
