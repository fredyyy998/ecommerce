package com.example.offersb.models;

import jakarta.annotation.Nullable;
import jakarta.persistence.*;
import lombok.Getter;
import lombok.NoArgsConstructor;

import java.time.LocalDateTime;
import java.util.UUID;

@Entity
@Inheritance(strategy = InheritanceType.SINGLE_TABLE)
@NoArgsConstructor
public abstract class Offer {

    @Id
    @Getter
    protected UUID id;

    @Getter
    protected String name;

    @Embedded
    @Getter
    protected Price price;

    @Embedded
    @Getter
    @Nullable
    protected Discount discount;

    @Getter
    protected LocalDateTime startDate;

    @Getter
    protected LocalDateTime endDate;

    @Getter
    @ManyToOne
    @JoinColumn(name = "localization_id")
    private Localization localization;

    protected Offer(UUID id, String name, Price price, LocalDateTime startDate, LocalDateTime endDate, Localization localization) {
        this.id = id;
        this.name = name;
        this.price = price;
        this.startDate = startDate;
        this.endDate = endDate;
        this.discount = new Discount();
        this.localization = localization;
    }

    public void applyDiscount(Discount discount) {
        if (discount == null) {
            throw new IllegalArgumentException("discount must not be null");
        }
        this.discount = discount;
        this.price = calculatePriceFromDiscount(discount, this.price);
    }

    private Price calculatePriceFromDiscount(Discount discount, Price price) {
        var discountPrice = discount.getDiscountRate() / 100 * price.getGrossPrice();
        return Price.CreateFromGross(price.getGrossPrice() - discountPrice, price.getTaxRate());
    }

    public void removeDiscount() {
        this.price = removeDiscountFromPrice(this.discount, this.price);
        this.discount = null;
    }

    private Price removeDiscountFromPrice(Discount discount, Price price) {
        var discountPrice = discount.getDiscountRate() / (100 - discount.getDiscountRate()) * price.getGrossPrice();
        return Price.CreateFromGross(price.getGrossPrice() + discountPrice, price.getTaxRate());
    }

}
