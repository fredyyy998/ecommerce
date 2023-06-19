package com.example.offersb.models;

import jakarta.annotation.Nullable;
import lombok.Getter;
import lombok.NoArgsConstructor;

import java.time.LocalDateTime;
import java.util.UUID;

@NoArgsConstructor
public abstract class Offer {

    @Getter
    protected UUID id;

    @Getter
    protected String name;

    @Getter
    protected Price price;

    @Getter
    @Nullable
    protected Discount discount;

    @Getter
    protected LocalDateTime startDate;

    @Getter
    protected LocalDateTime endDate;

    protected Offer(UUID id, String name, Price price, LocalDateTime startDate, LocalDateTime endDate) {
        this.id = id;
        this.name = name;
        this.price = price;
        this.startDate = startDate;
        this.endDate = endDate;
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
        return Price.CreateFromGross(price.getGrossPrice() - discountPrice, price.getTaxRate(), price.getCurrencyCode());
    }

    public void removeDiscount() {
        this.price = removeDiscountFromPrice(this.discount, this.price);
        this.discount = null;
    }

    private Price removeDiscountFromPrice(Discount discount, Price price) {
        var discountPrice = discount.getDiscountRate() / (100 - discount.getDiscountRate()) * price.getGrossPrice();
        return Price.CreateFromGross(price.getGrossPrice() + discountPrice, price.getTaxRate(), price.getCurrencyCode());
    }

}
