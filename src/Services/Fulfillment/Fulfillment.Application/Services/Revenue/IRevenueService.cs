using Fulfillment.Application.Dtos;

namespace Fulfillment.Application.Services.Revenue;

public interface IRevenueService
{
    Task<RevenueReportResponseDto> GetRevenueReportAsync(RevenueQuery query);
}