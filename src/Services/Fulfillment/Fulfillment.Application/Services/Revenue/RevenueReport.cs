using AutoMapper;
using Fulfillment.Application.Dtos;
using Fulfillment.Core.Order;

namespace Fulfillment.Application.Services.Revenue;

public class RevenueReport : IRevenueService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;
    
    public RevenueReport(IOrderRepository orderRepository, IMapper mapper)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
    }

    public async Task<RevenueReportResponseDto> GetRevenueReportAsync(DateTime startDate, DateTime endDate)
    {
        var orders = await _orderRepository.FindInDateRangeAsync(startDate, endDate);
        var revenueReport = Core.Revenue.RevenueReport.CreateRevenueReport(orders);
        return _mapper.Map<RevenueReportResponseDto>(revenueReport);
    }
}