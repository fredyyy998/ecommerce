package com.example.offersb.Dtos;

import java.time.LocalDateTime;
import java.util.List;
import java.util.UUID;

public record CreatePackageOfferRequestDto(
        String name,
        double grossPrice,
        double taxValue,
        String currency,
        LocalDateTime startDate,
        LocalDateTime endDate,
        List<UUID> productIds
) {
}
