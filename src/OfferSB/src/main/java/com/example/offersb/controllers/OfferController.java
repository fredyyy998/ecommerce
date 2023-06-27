package com.example.offersb.controllers;

import com.example.offersb.Dtos.CreatePackageOfferRequestDto;
import com.example.offersb.Dtos.CreateSingleOfferRequestDto;
import com.example.offersb.models.*;
import com.example.offersb.repositories.ILocalizationRepository;
import com.example.offersb.repositories.IOfferRepository;
import com.example.offersb.repositories.IProductRepository;
import org.springframework.web.bind.annotation.*;

import java.util.ArrayList;
import java.util.List;
import java.util.UUID;

@RestController
@RequestMapping("/api/offers")
public class OfferController {

    private final IOfferRepository offerRepository;

    private final IProductRepository productRepository;

    private final ILocalizationRepository localizationRepository;

    public OfferController(IOfferRepository offerRepository, IProductRepository productRepository, ILocalizationRepository localizationRepository) {
        this.offerRepository = offerRepository;
        this.productRepository = productRepository;
        this.localizationRepository = localizationRepository;
    }

    @GetMapping()
    public List<Offer> getAllOffers() {
        return offerRepository.findAll();
    }

    @GetMapping("/{id}")
    public Offer getOfferById(@PathVariable("id") UUID id) {
        return offerRepository.findById(id).orElse(null);
    }

    @PostMapping("single")
    public SingleOffer createSingleOffer(@RequestBody CreateSingleOfferRequestDto offerDto) {
        var price = Price.CreateFromGross(offerDto.grossPrice(), offerDto.taxValue());
        var product = productRepository.findById(offerDto.productId()).orElse(null);
        var localization = localizationRepository.findById("DE");
        var offer = SingleOffer.create(UUID.randomUUID(), offerDto.name(), price, offerDto.startDate(), offerDto.endDate(), product, localization.get());
        return offerRepository.save(offer);
    }

    @PostMapping("package")
    public PackageOffer createPackageOffer(@RequestBody CreatePackageOfferRequestDto offerDto) {
        var price = Price.CreateFromGross(offerDto.grossPrice(), offerDto.taxValue());

        var products = new ArrayList<Product>();
        for (var productId : offerDto.productIds()) {
            var product = productRepository.findById(productId).orElse(null);
            products.add(product);
        }
        var localization = localizationRepository.findById("DE");
        var offer = PackageOffer.create(UUID.randomUUID(), offerDto.name(), price, offerDto.startDate(), offerDto.endDate(), products, localization.get());
        return offerRepository.save(offer);
    }

    @DeleteMapping("/{id}")
    public void deleteOffer(@PathVariable("id") UUID id) {
        offerRepository.deleteById(id);
    }
}
