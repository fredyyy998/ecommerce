package com.example.offersb.models;

import jakarta.persistence.Embeddable;
import lombok.Getter;
import lombok.NoArgsConstructor;

@Embeddable
@NoArgsConstructor
public class Price {

    @Getter
    private double grossPrice;

    @Getter
    private double netPrice;

    @Getter
    private double taxRate;

    private Price(double grossPrice, double netPrice, double taxRate) {
        this.grossPrice = grossPrice;
        this.netPrice = netPrice;
        this.taxRate = taxRate;
    }

    public static Price CreateFromGross(double grossPrice, double taxRate) {
        if (grossPrice < 0) {
            throw new IllegalArgumentException("grossPrice must be greater than or equal to 0");
        }
        if (taxRate < 0) {
            throw new IllegalArgumentException("taxRate must be greater than or equal to 0");
        }

        double netPrice = grossPrice / (1 + taxRate / 100);
        return new Price(grossPrice, netPrice, taxRate);
    }

}
