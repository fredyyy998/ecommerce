package com.example.offersb.models;

import lombok.NoArgsConstructor;

import jakarta.persistence.*;
import java.time.LocalDateTime;
import java.util.Collections;
import java.util.List;
import java.util.UUID;

@NoArgsConstructor
@Entity
public class PackageOffer extends Offer {

    @ManyToMany
    protected List<Product> products;

    public List<Product> getProducts() {
        return (List<Product>) Collections.unmodifiableCollection(products);
    }

    protected PackageOffer(UUID id, String name, Price price, LocalDateTime startDate, LocalDateTime endDate, List<Product> products) {
        super(id, name, price, startDate, endDate);
        this.products = products;
    }

    public static PackageOffer create(UUID id, String name, Price price, LocalDateTime startDate, LocalDateTime endDate, List<Product> products) {
        return new PackageOffer(id, name, price, startDate, endDate, products);
    }
}
