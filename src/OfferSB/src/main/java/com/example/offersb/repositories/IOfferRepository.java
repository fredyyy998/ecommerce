package com.example.offersb.repositories;

import com.example.offersb.models.Offer;
import com.example.offersb.models.PackageOffer;
import com.example.offersb.models.SingleOffer;
import org.springframework.data.domain.PageRequest;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;

import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.List;
import java.util.UUID;

public interface IOfferRepository extends JpaRepository<Offer, UUID> {

    List<Offer> findAllBy(PageRequest pageable);
    List<Offer> findByEndDateGreaterThan(LocalDateTime date);

    List<Offer> findByEndDateLessThan(LocalDateTime date);

    @Query("SELECT o FROM SingleOffer o WHERE o.product.id = :productId")
    List<Offer> findSingleOffersByProductId(UUID productId);

    @Query("SELECT o FROM PackageOffer o JOIN o.products p WHERE p.id = :productId")
    List<Offer> findPackageOffersByProductId(UUID productId);

    default List<Offer> findByProductId(UUID productId) {
        var singleOffers = findSingleOffersByProductId(productId);
        var packageOffers = findPackageOffersByProductId(productId);
        singleOffers.addAll(packageOffers);
        return singleOffers;
    }

}