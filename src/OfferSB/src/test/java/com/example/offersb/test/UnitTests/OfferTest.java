package com.example.offersb.test.UnitTests;

import com.example.offersb.models.*;
import org.junit.jupiter.api.Assertions;
import org.junit.jupiter.api.Test;

import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.UUID;

public class OfferTest {

    @Test
    public void testAdd_Discount_To_Offer() {
        var price = Price.CreateFromGross(119, 19);
        var product = Product.create(UUID.randomUUID(), "Test Product", "Test Product Description");
        var localization = Localization.create("DE", "Germany", "Deutschland", "EUR");
        var offer = SingleOffer.create("Test Offer", price, LocalDateTime.now(), LocalDateTime.MAX, product, localization);

        var discount = Discount.create(10, LocalDateTime.now(), LocalDateTime.MAX);
        offer.applyDiscount(discount);

        assert offer.getDiscount() != null;
        assert offer.getPrice().getGrossPrice() == 107.1;
        assert offer.getPrice().getNetPrice() == 90;
    }

    @Test
    public void testRemove_Discount_From_Offer() {
        var price = Price.CreateFromGross(119, 19);
        var product = Product.create(UUID.randomUUID(), "Test Product", "Test Product Description");
        var localization = Localization.create("DE", "Germany", "Deutschland", "EUR");
        var offer = SingleOffer.create("Test Offer", price, LocalDateTime.now(), LocalDateTime.MAX, product, localization);
        var discount = Discount.create(10, LocalDateTime.now(), LocalDateTime.MAX);
        offer.applyDiscount(discount);

        offer.removeDiscount();

        assert offer.getDiscount() == null;
        assert offer.getPrice().getGrossPrice() == 119;
        assert offer.getPrice().getNetPrice() == 100;
    }

    @Test
    public void testPackageOffer_Is_Instantiated_Correctly() {
        var price = Price.CreateFromGross(119, 19);
        var product1 = Product.create(UUID.randomUUID(), "test product", "test description");
        var product2 = Product.create(UUID.randomUUID(), "test product2", "test description");
        var localization = Localization.create("DE", "Germany", "Deutschland", "EUR");
        var products = new ArrayList<Product>(Arrays.asList(product1, product2));
        var dateStart = LocalDateTime.now();
        var dateEnd = LocalDateTime.MAX;
        var offer = PackageOffer.create("Test Offer", price, dateStart, dateEnd, products, localization);

        assert offer.getId() != null;
        assert offer.getName() == "Test Offer";
        assert offer.getPrice() == price;
        assert offer.getStartDate() == dateStart;
        assert offer.getEndDate() == dateEnd;
        assert offer.getProducts() == products;
    }

    @Test
    public void testCannot_Apply_2_Discounts_To_Offer() {
        var price = Price.CreateFromGross(119, 19);
        var product = Product.create(UUID.randomUUID(), "Test Product", "Test Product Description");
        var localization = Localization.create("DE", "Germany", "Deutschland", "EUR");
        var offer = SingleOffer.create("Test Offer", price, LocalDateTime.now(), LocalDateTime.MAX, product, localization);
        var discount1 = Discount.create(10, LocalDateTime.now(), LocalDateTime.MAX);
        offer.applyDiscount(discount1);

        var discount2 = Discount.create(10, LocalDateTime.now(), LocalDateTime.MAX);

        Assertions.assertThrows(IllegalStateException.class, () -> offer.applyDiscount(discount2));
    }
}
