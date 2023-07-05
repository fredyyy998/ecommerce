package com.example.offersb.models;

import jakarta.persistence.*;
import lombok.Getter;
import lombok.NoArgsConstructor;
import org.hibernate.annotations.Cascade;

import java.time.LocalDateTime;
import java.util.UUID;

@Entity
@NoArgsConstructor
public class SingleOffer extends Offer {

    @Getter
    @ManyToOne(cascade = {CascadeType.PERSIST, CascadeType.MERGE})
    protected Product product;

    protected SingleOffer(UUID id, String name, Price price, LocalDateTime startDate, LocalDateTime endDate, Product product, Localization localization) {
        super(id, name, price, startDate, endDate, localization);
        this.product = product;
    }

    public static SingleOffer create(UUID id, String name, Price price, LocalDateTime startDate, LocalDateTime endDate, Product product, Localization localization) {
        return new SingleOffer(id, name, price, startDate, endDate, product, localization);
    }

    public static SingleOffer create(String name, Price price, LocalDateTime startDate, LocalDateTime endDate, Product product, Localization localization) {
        return new SingleOffer(UUID.randomUUID(), name, price, startDate, endDate, product, localization);
    }
}
