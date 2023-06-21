package com.example.offersb.models;

import jakarta.persistence.Column;
import jakarta.persistence.Embeddable;
import lombok.Getter;
import lombok.NoArgsConstructor;

import java.time.LocalDateTime;

@Embeddable
@NoArgsConstructor
public class Discount {

    @Getter
    private double discountRate;

    @Getter
    @Column(name = "discount_start_date")
    private LocalDateTime startDate;

    @Getter
    @Column(name = "discount_end_date")
    private LocalDateTime endDate;

    private Discount(double discountRate, LocalDateTime startDate, LocalDateTime endDate) {
        this.discountRate = discountRate;
        this.startDate = startDate;
        this.endDate = endDate;
    }

    public static Discount create(double discountRate, LocalDateTime startDate, LocalDateTime endDate) {
        return new Discount(discountRate, startDate, endDate);
    }
}
