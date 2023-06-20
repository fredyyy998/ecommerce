package com.example.offersb.repositories;

import com.example.offersb.models.Offer;
import com.example.offersb.models.PackageOffer;
import com.example.offersb.models.SingleOffer;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;

import java.time.LocalDate;
import java.util.List;
import java.util.UUID;

public interface IOfferRepository extends JpaRepository<Offer, UUID> {

    List<Offer> findByEndDateGreaterThan(LocalDate date);

    List<Offer> findByEndDateLessThan(LocalDate date);

    // @Query("SELECT so FROM SingleOffer so JOIN so.product p WHERE p.id = :productId " +
    //         "UNION " +
    //         "SELECT po FROM PackageOffer po JOIN po.products p WHERE p.id = :productId")
    // List<Offer> findByProductId(@Param("productId") Long productId);
}
