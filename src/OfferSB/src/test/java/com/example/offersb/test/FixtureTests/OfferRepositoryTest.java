package com.example.offersb.test.FixtureTests;

import com.example.offersb.models.Discount;
import com.example.offersb.models.Price;
import com.example.offersb.models.Product;
import com.example.offersb.models.SingleOffer;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.params.ParameterizedTest;
import org.junit.jupiter.params.provider.CsvSource;
import org.springframework.data.domain.PageRequest;

import java.time.LocalDateTime;
import java.util.UUID;

public class OfferRepositoryTest extends TestBaseClass {

    @ParameterizedTest
    @CsvSource({
            "0, 6, 6",
            "1, 3, 3",
            "1, 4, 2",
            "1, 6, 0",
    })
    public void testFind_All_Offers(int page, int pageSize, int resultCount) {
        // arrange
        var pageable = PageRequest.of(page, pageSize);

        // act
        var offers = offerRepository.findAllBy(pageable);

        // assert
        assert offers.size() == resultCount;
    }

    @Test
    public void Find_All_Available_Offers() {
        // act
        var offers = offerRepository.findByEndDateGreaterThan(LocalDateTime.now());

        // assert
        assert offers.size() == 4;
    }

    @Test
    public void Find_All_Expired_Offers() {
        // act
        var offers = offerRepository.findByEndDateLessThan(LocalDateTime.now());

        // assert
        assert offers.size() == 2;
    }

    @Test
    public void Find_By_Id_Returns_Correct_When_Exists() {
        // act
        var offer = offerRepository.findById(UUID.fromString("78e13d5e-da89-4d09-8249-dcef0858f924"));

        // assert
        assert offer.isPresent();
        assert offer.get().getName().equals("Offer 1");
    }

    @Test
    public void Find_By_Id_Returns_Empty_When_Not_Exists() {
        // act
        var offer = offerRepository.findById(UUID.fromString("78e13d5e-da89-4d09-8249-dcef0858f925"));

        // assert
        assert offer.isEmpty();
    }

    @Test
    public void Finds_Offers_By_Product() {
        // act
        var offers = offerRepository.findByProductId(UUID.fromString("07d6faf7-ad39-4810-82f7-1f4b40e3c877"));

        // assert
        assert offers.size() == 3;
    }

    @Test
    public void Persists_New_Offer_And_Returns_It() {
        // arrange
        var price = Price.CreateFromGross(10, 19);
        var localisation = localizationRepository.findById("DE").get();
        var product = productRepository.findById(UUID.fromString("e2bf10b3-ea7f-47a4-a70e-160fd1b4abe5")).get();
        var offer = SingleOffer.create( "Offer New", price, LocalDateTime.now().minusDays(10), LocalDateTime.now().plusDays(10), product, localisation);

        // act
        var persistedOffer = offerRepository.save(offer);

        // assert
        assert persistedOffer.getId() != null;
        assert persistedOffer.getName().equals("Offer New");
        assert offerRepository.findAll().size() == 7;
    }

    @Test
    public void Persists_New_Offer_With_new_Product() {
        // arrange
        var price = Price.CreateFromGross(10, 19);
        var localisation = localizationRepository.findById("DE").get();
        var product = Product.create(UUID.fromString("f1273588-40fd-4050-b956-a61ae82db4d1"), "Prod name", "description");
        var offer = SingleOffer.create( "Offer New", price, LocalDateTime.now().minusDays(10), LocalDateTime.now().plusDays(10), product, localisation);

        // act
        var persistedOffer = offerRepository.save(offer);

        // assert
        var persistedProduct = productRepository.findById(UUID.fromString("f1273588-40fd-4050-b956-a61ae82db4d1")).get();
        assert persistedProduct.getName().equals("Prod name");
        assert persistedProduct.getId().equals(product.getId());

        assert offerRepository.findAll().size() == 7;
    }

    @Test
    public void Persists_Update() {
        // arrange
        var offer = offerRepository.findById(UUID.fromString("78e13d5e-da89-4d09-8249-dcef0858f924")).get();

        // act
        var discount = Discount.create(10, LocalDateTime.now(), LocalDateTime.now().plusDays(10));
        offer.applyDiscount(discount);
        offerRepository.save(offer);

        // assert
        var persistedOffer = offerRepository.findById(UUID.fromString("78e13d5e-da89-4d09-8249-dcef0858f924")).get();
        assert persistedOffer.getDiscount().getDiscountRate() == 10;
    }

    @Test
    public void Removes_Offer() {
        // arrange
        var offer = offerRepository.findById(UUID.fromString("78e13d5e-da89-4d09-8249-dcef0858f924")).get();

        // act
        offerRepository.delete(offer);

        // assert
        assert offerRepository.findAll().size() == 5;
    }
}
