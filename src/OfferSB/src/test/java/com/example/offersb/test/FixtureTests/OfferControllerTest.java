package com.example.offersb.test.FixtureTests;

import com.example.offersb.Dtos.CreateSingleOfferRequestDto;
import com.example.offersb.controllers.OfferController;
import com.example.offersb.models.*;
import com.example.offersb.repositories.ILocalizationRepository;
import com.example.offersb.repositories.IOfferRepository;
import com.example.offersb.repositories.IProductRepository;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.context.SpringBootTest;

import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.UUID;

@SpringBootTest
public class OfferControllerTest extends TestBaseClass {


    @Test
    public void testgetAllOffers_Should_Return_All_Offers() {
        // arrange
        var controller = new OfferController(offerRepository, productRepository, localizationRepository);

        // act
        var result = controller.getAllOffers(0, 10);

        // assert
        assert result != null;
        assert result.size() == 6;
    }

    @Test
    public void test_CreateSingleOffer_Should_be_Persisted() {
        // arrange
        var controller = new OfferController(offerRepository, productRepository, localizationRepository);
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
