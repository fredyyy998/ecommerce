using AutoMapper;
using Fulfillment.Application.Dtos;
using Fulfillment.Core.Order;

namespace Fulfillment.Application.Services.Revenue;

public class RevenueReportService : IRevenueService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;
    
    public RevenueReportService(IOrderRepository orderRepository, IMapper mapper)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
    }

    public async Task<RevenueReportResponseDto> GetRevenueReportAsync(RevenueQuery query)
    {
        var orders = await _orderRepository.FindInDateRangeAsync(query.StartDate, query.EndDate);
        var revenueReport = Core.Revenue.RevenueReport.CreateRevenueReport(orders);
        return _mapper.Map<RevenueReportResponseDto>(revenueReport);
    }
}