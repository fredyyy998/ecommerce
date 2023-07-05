package com.example.offersb.test.MockedTests;

import com.example.offersb.Dtos.CreateSingleOfferRequestDto;
import com.example.offersb.controllers.OfferController;
import com.example.offersb.models.*;
import com.example.offersb.repositories.ILocalizationRepository;
import com.example.offersb.repositories.IOfferRepository;
import com.example.offersb.repositories.IProductRepository;
import org.junit.jupiter.api.Test;
import org.springframework.data.domain.PageRequest;

import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.UUID;

import static org.mockito.Mockito.*;

public class OfferControllerMockTest {
    @Test
    public void testgetAllOffers_Should_Return_All_Offers() {
        // arrange
        IProductRepository mockProductRepository = mock(IProductRepository.class);
        IOfferRepository mockOfferRepository = mock(IOfferRepository.class);
        ILocalizationRepository mockLocalizationRepository = mock(ILocalizationRepository.class);
        var offers = new ArrayList<Offer>();
        var price = Price.CreateFromGross(10, 19);
        var product = Product.create(UUID.randomUUID(), "Product 1", "Product 1 description");
        var localization = Localization.create("DE", "Germany", "Deutschland", "EUR");
        offers.add(SingleOffer.create("Offer 1", price, LocalDateTime.now(), LocalDateTime.now().plusDays(1), product, localization));
        var pageable = PageRequest.of(0, 10);
        when(mockOfferRepository.findAllBy(pageable)).thenReturn(offers);

        var controller = new OfferController(mockOfferRepository, mockProductRepository, mockLocalizationRepository);

        // act
        var result = controller.getAllOffers(0, 10);

        // assert
        assert result != null;
        assert result.size() == 1;
    }

    @Test
    public void test_CreateSingle_Offer_Calls_Persistence() {
        // arrange
        IProductRepository mockProductRepository = mock(IProductRepository.class);
        IOfferRepository mockOfferRepository = mock(IOfferRepository.class);
        ILocalizationRepository mockLocalizationRepository = mock(ILocalizationRepository.class);
        when(mockLocalizationRepository.findById("DE")).thenReturn(java.util.Optional.of(Localization.create("DE", "Germany", "Deutschland", "EUR")));
        var controller = new OfferController(mockOfferRepository, mockProductRepository, mockLocalizationRepository);
        var product = Product.create(UUID.randomUUID(), "Product 2", "Description");
        var request = new CreateSingleOfferRequestDto("Offer 2", 10, 19, "EUR", LocalDateTime.now(), LocalDateTime.now().plusDays(1),  product.getId());
        when(mockProductRepository.findById(product.getId())).thenReturn(java.util.Optional.of(product));

        // act
       controller.createSingleOffer(request);

        // assert
        verify(mockOfferRepository, times(1)).save(any());
    }
}
