package com.example.offersb.test.FixtureTests;

import com.example.offersb.models.*;
import com.example.offersb.repositories.ILocalizationRepository;
import com.example.offersb.repositories.IOfferRepository;
import com.example.offersb.repositories.IProductRepository;
import org.junit.jupiter.api.BeforeEach;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.context.SpringBootTest;

import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.UUID;

@SpringBootTest
public class TestBaseClass {


    @Autowired
    protected IOfferRepository offerRepository;

    @Autowired
    protected IProductRepository productRepository;

    @Autowired
    protected ILocalizationRepository localizationRepository;

    @BeforeEach
    public void cleanDatabase() {
        offerRepository.deleteAll();
        productRepository.deleteAll();

        setupData();
    }

    protected void setupData() {
        var localisation = Localization.create("DE", "Germany", "Deutschland", "EUR");

        var products = new ArrayList<Product>();
        products.add(Product.create(UUID.fromString("e2bf10b3-ea7f-47a4-a70e-160fd1b4abe5"), "Product 1", "Description"));
        products.add(Product.create(UUID.fromString("c2511115-cb17-4deb-8c00-d5b1b787327a"), "Product 2", "Description"));
        products.add(Product.create(UUID.fromString("78fb0cae-4b1b-417b-91c3-f483fce69761"), "Product 3", "Description"));
        products.add(Product.create(UUID.fromString("07d6faf7-ad39-4810-82f7-1f4b40e3c877"), "Product 4", "Description"));
        products.add(Product.create(UUID.fromString("43ff93b2-3272-4dbf-bcd3-263212e7a7f6"), "Product 5", "Description"));
        productRepository.saveAll(products);

        var price = Price.CreateFromGross(10, 19);
        var offer = SingleOffer.create(UUID.fromString("78e13d5e-da89-4d09-8249-dcef0858f924"), "Offer 1", price, LocalDateTime.now(), LocalDateTime.now().plusDays(1), products.get(4), localisation);
        var price2 = Price.CreateFromGross(10, 19);
        var offer2 = SingleOffer.create(UUID.fromString("7d67521c-6a66-428d-918d-d77a7780892d"),"Offer 2", price2, LocalDateTime.now().minusDays(10), LocalDateTime.now().minusDays(5),  products.get(3), localisation);
        var price3 = Price.CreateFromGross(10, 19);
        var offer3 = SingleOffer.create(UUID.fromString("c216d6f5-c2cf-4ada-b816-13a23c0cce70"), "Offer 3", price3, LocalDateTime.now().minusDays(10), LocalDateTime.now().minusDays(5), products.get(1), localisation);
        var price4 = Price.CreateFromGross(10, 19);
        var offer4 = SingleOffer.create(UUID.fromString("e36f7dc3-ea03-41e8-bbb1-508e970f43bd"), "Offer 4", price4, LocalDateTime.now().minusDays(10), LocalDateTime.now().plusDays(10), products.get(3), localisation);

        var price5 = Price.CreateFromGross(10, 19);
        var offer5 = PackageOffer.create(UUID.fromString("ac0b52d3-54cd-4572-b32d-6f65aed7e610"),"Package Offer 1", price5, LocalDateTime.now().minusDays(10), LocalDateTime.now().plusDays(15), new ArrayList<Product>(Arrays.asList(products.get(3), products.get(4))), localisation);
        var price6 = Price.CreateFromGross(10, 19);
        var offer6 = PackageOffer.create(UUID.fromString("d40c43d3-7285-4fab-aea4-9343e99e6bb6"),"Package Offer 2", price6, LocalDateTime.now().minusDays(10), LocalDateTime.now().plusDays(2), new ArrayList<Product>(Arrays.asList(products.get(1), products.get(4))), localisation);

        offerRepository.saveAll(Arrays.asList(offer, offer2, offer3, offer4, offer5, offer6));
    }
}
