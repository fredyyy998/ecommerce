package com.example.offersb;

import com.example.offersb.models.*;
import com.example.offersb.repositories.ILocalizationRepository;
import com.example.offersb.repositories.IOfferRepository;
import com.example.offersb.repositories.IProductRepository;
import jakarta.annotation.PostConstruct;
import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;

import java.time.LocalDateTime;
import java.util.ArrayList;
import java.util.UUID;

@SpringBootApplication
public class OfferSbApplication {
    private final IProductRepository productRepository;
    private final IOfferRepository offerRepository;

    private final ILocalizationRepository localizationRepository;

    public OfferSbApplication(IProductRepository productRepository, IOfferRepository offerRepository, ILocalizationRepository localizationRepository) {
        this.productRepository = productRepository;
        this.offerRepository = offerRepository;
        this.localizationRepository = localizationRepository;
    }
    public static void main(String[] args) {
        SpringApplication.run(OfferSbApplication.class, args);
    }

    @PostConstruct
    public void init() {
        var products = new ArrayList<Product>();
        products.add(Product.create(UUID.fromString("fd239078-28ff-4158-a79c-ad533ce26dc5"), "Product 1", "Product 1 description"));
        products.add(Product.create(UUID.fromString("b7c196c6-e082-460f-af7f-0262f960d2ee"), "Product 2", "Product 2 description"));
        products.add(Product.create(UUID.fromString("a7948abe-571c-4e9d-a7f3-94a88ed10125"), "Product 3", "Product 3 description"));
        productRepository.saveAll(products);


        var localizations = localizationRepository.findById("DE");
        var offers = new ArrayList<Offer>();
        var price1 = Price.CreateFromGross(119, 19);
        offers.add(SingleOffer.create(UUID.fromString("ad8b5807-cab4-4006-9425-1596bcf55c4a"), "Offer 1", price1, LocalDateTime.now(), LocalDateTime.now(), products.get(0), localizations.get()));

        var price2 = Price.CreateFromGross(99, 19);
        offers.add(PackageOffer.create(UUID.fromString("d4495823-04a4-4ff9-9cc9-f17adee419f7"), "Offer 2", price2, LocalDateTime.now(), LocalDateTime.now(), products, localizations.get()));
        offerRepository.saveAll(offers);
    }
}
