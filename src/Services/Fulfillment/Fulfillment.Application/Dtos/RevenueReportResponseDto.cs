namespace Fulfillment.Application.Dtos;

public record RevenueReportResponseDto(
    DateTime StartDate,
    DateTime EndDate,
    decimal Revenue);