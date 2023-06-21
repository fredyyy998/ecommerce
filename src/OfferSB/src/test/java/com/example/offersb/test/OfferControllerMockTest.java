package com.example.offersb.test;

import com.example.offersb.Dtos.CreateSingleOfferRequestDto;
import com.example.offersb.controllers.OfferController;
import com.example.offersb.models.Offer;
import com.example.offersb.models.Price;
import com.example.offersb.models.Product;
import com.example.offersb.models.SingleOffer;
import com.example.offersb.repositories.IOfferRepository;
import com.example.offersb.repositories.IProductRepository;
import org.junit.jupiter.api.Test;

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
        var offers = new ArrayList<Offer>();
        var price = Price.CreateFromGross(119, 19, "EUR");
        var product = Product.create(UUID.randomUUID(), "Product 1", "Product 1 description");
        offers.add(SingleOffer.create(UUID.randomUUID(), "Name", price, LocalDateTime.now(), LocalDateTime.now(), product));
        when(mockOfferRepository.findAll()).thenReturn(offers);

        var controller = new OfferController(mockOfferRepository, mockProductRepository);

        // act
        var result = controller.getAllOffers();

        // assert
        assert result != null;
        assert result.size() == 1;
    }

    @Test
    public void test_CreateSingle_Offer_Calls_Persistence() {
        // arrange
        IProductRepository mockProductRepository = mock(IProductRepository.class);
        IOfferRepository mockOfferRepository = mock(IOfferRepository.class);
        var controller = new OfferController(mockOfferRepository, mockProductRepository);
        var product = Product.create(UUID.randomUUID(), "Product 1", "Product 1 description");
        var request = new CreateSingleOfferRequestDto("Name", 119, 19, "EUR", LocalDateTime.now(), LocalDateTime.now(),  product.getId());
        when(mockProductRepository.findById(product.getId())).thenReturn(java.util.Optional.of(product));

        // act
        controller.createSingleOffer(request);

        // assert
        verify(mockOfferRepository, times(1)).save(any());

    }
}
