package com.example.offersb.test;

import com.example.offersb.models.Discount;
import com.example.offersb.models.Price;
import com.example.offersb.models.Product;
import com.example.offersb.models.SingleOffer;
import org.junit.jupiter.api.Test;

import java.time.LocalDateTime;
import java.util.UUID;

public class OfferTest {

    @Test
    public void testAdd_Discount_To_Offer() {
        var price = Price.CreateFromGross(119, 19, "EUR");
        var product = Product.create(UUID.randomUUID(), "Test Product", "Test Product Description");
        var offer = SingleOffer.create(UUID.randomUUID(), "Test Offer", price, LocalDateTime.now(), LocalDateTime.MAX, product);

        var discount = Discount.create(10, LocalDateTime.now(), LocalDateTime.MAX);
        offer.applyDiscount(discount);

        assert offer.getDiscount() != null;
        assert offer.getPrice().getGrossPrice() == 107.1;
        assert offer.getPrice().getNetPrice() == 90;
    }

    @Test
    public void testRemove_Discount_From_Offer() {
        var price = Price.CreateFromGross(119, 19, "EUR");
        var product = Product.create(UUID.randomUUID(), "Test Product", "Test Product Description");
        var offer = SingleOffer.create(UUID.randomUUID(), "Test Offer", price, LocalDateTime.now(), LocalDateTime.MAX, product);
        var discount = Discount.create(10, LocalDateTime.now(), LocalDateTime.MAX);
        offer.applyDiscount(discount);

        offer.removeDiscount();

        assert offer.getDiscount() == null;
        assert offer.getPrice().getGrossPrice() == 119;
        assert offer.getPrice().getNetPrice() == 100;
    }
}
