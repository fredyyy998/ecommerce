package com.example.offersb.models;

import jakarta.persistence.Entity;
import jakarta.persistence.ManyToOne;
import lombok.Getter;
import lombok.NoArgsConstructor;

import java.time.LocalDateTime;
import java.util.UUID;

@Entity
@NoArgsConstructor
public class SingleOffer extends Offer {

    @Getter
    @ManyToOne
    protected Product product;

    protected SingleOffer(UUID id, String name, Price price, LocalDateTime startDate, LocalDateTime endDate, Product product) {
        super(id, name, price, startDate, endDate);
        this.product = product;
    }

    public static SingleOffer create(UUID id, String name, Price price, LocalDateTime startDate, LocalDateTime endDate, Product product) {
        return new SingleOffer(id, name, price, startDate, endDate, product);
    }
}
