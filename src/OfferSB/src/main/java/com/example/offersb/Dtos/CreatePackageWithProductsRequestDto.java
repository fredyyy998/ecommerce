package com.example.offersb.Dtos;

import java.time.LocalDateTime;
import java.util.List;

public record CreatePackageWithProductsRequestDto(String name,
                                                  double grossPrice,
                                                  double taxValue,
                                                  String currency,
                                                  LocalDateTime startDate,
                                                  LocalDateTime endDate,
                                                  List<CreateProductRequestDto> products) {
}
