package com.example.offersb.test;

import com.example.offersb.Dtos.CreateSingleOfferRequestDto;
import com.example.offersb.controllers.OfferController;
import com.example.offersb.models.*;
import com.example.offersb.repositories.IOfferRepository;
import com.example.offersb.repositories.IProductRepository;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.context.SpringBootTest;

import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.UUID;

@SpringBootTest
public class OfferControllerTest {

    @Autowired
    private IOfferRepository offerRepository;

    @Autowired
    private IProductRepository productRepository;

    @BeforeEach
    public void cleanDatabase() {
        offerRepository.deleteAll();
        productRepository.deleteAll();

        setupData();
    }

    private void setupData() {
        var products = new ArrayList<Product>();
        products.add(Product.create(UUID.fromString("fd239078-28ff-4158-a79c-ad533ce26dc5"), "Product 1", "Product 1 description"));
        products.add(Product.create(UUID.fromString("b7c196c6-e082-460f-af7f-0262f960d2ee"), "Product 2", "Product 2 description"));
        products.add(Product.create(UUID.fromString("a7948abe-571c-4e9d-a7f3-94a88ed10125"), "Product 3", "Product 3 description"));
        productRepository.saveAll(products);

        var offers = new ArrayList<Offer>();
        var price1 = Price.CreateFromGross(119, 19, "EUR");
        offers.add(SingleOffer.create(UUID.fromString("ad8b5807-cab4-4006-9425-1596bcf55c4a"), "Offer 1", price1, LocalDateTime.now(), LocalDateTime.now(), products.get(0)));

        var price2 = Price.CreateFromGross(99, 19, "EUR");
        offers.add(PackageOffer.create(UUID.fromString("d4495823-04a4-4ff9-9cc9-f17adee419f7"), "Offer 2", price2, LocalDateTime.now(), LocalDateTime.now(), products));
        offerRepository.saveAll(offers);
    }

    @Test
    public void testgetAllOffers_Should_Return_All_Offers() {
        // arrange
        var controller = new OfferController(offerRepository, productRepository);

        // act
        var result = controller.getAllOffers();

        // assert
        assert result != null;
        assert result.size() == 2;
    }

    @Test
    public void test_CreateSingleOffer_Should_be_Persisted() {
        // arrange
        var controller = new OfferController(offerRepository, productRepository);
        var offerDto = new CreateSingleOfferRequestDto("Offer 1", 119, 19, "EUR", LocalDateTime.now(), LocalDateTime.now(), UUID.fromString("fd239078-28ff-4158-a79c-ad533ce26dc5"));
        // act
        var result = controller.createSingleOffer(offerDto);

        // assert
        assert result != null;
        var persistedOffer = offerRepository.findById(result.getId());
        assert persistedOffer.isPresent();
        assert persistedOffer.get().getName().equals("Offer 1");
    }
}
