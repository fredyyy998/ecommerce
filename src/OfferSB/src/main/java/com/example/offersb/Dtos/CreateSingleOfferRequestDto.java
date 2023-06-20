package com.example.offersb.Dtos;

import java.time.LocalDateTime;
import java.util.UUID;

public record CreateSingleOfferRequestDto(
        String name,
        double grossPrice,
        double taxValue,
        String currency,
        LocalDateTime startDate,
        LocalDateTime endDate,
        UUID productId
) {
}

