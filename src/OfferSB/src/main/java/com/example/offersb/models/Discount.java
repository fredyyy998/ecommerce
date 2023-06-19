package com.example.offersb.models;

import lombok.Getter;

import java.time.LocalDateTime;

public class Discount {

    @Getter
    private double discountRate;

    @Getter
    public LocalDateTime startDate;

    @Getter
    public LocalDateTime endDate;

    private Discount(double discountRate, LocalDateTime startDate, LocalDateTime endDate) {
        this.discountRate = discountRate;
        this.startDate = startDate;
        this.endDate = endDate;
    }

    public static Discount create(double discountRate, LocalDateTime startDate, LocalDateTime endDate) {
        return new Discount(discountRate, startDate, endDate);
    }
}
