package com.example.offersb.Dtos;

import java.time.LocalDateTime;

public record CreateSingleOfferWithProductRequestDto(
        String name,
        double grossPrice,
        double taxValue,
        String currency,
        LocalDateTime startDate,
        LocalDateTime endDate,

        CreateProductRequestDto product
) {
}
