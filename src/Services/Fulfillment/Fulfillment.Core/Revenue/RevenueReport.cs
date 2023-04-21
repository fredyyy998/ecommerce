using Ecommerce.Common.Core;

namespace Fulfillment.Core.Revenue;

public class RevenueReport
{
    public DateTime StartDate { get; private set; }
    
    public DateTime EndDate { get; private set; }
    
    public decimal Revenue { get; private set; }

    public static RevenueReport CreateRevenueReport(List<Order.Order> orders)
    {
        var revenueReport = new RevenueReport();
        revenueReport.StartDate = orders.Min(o => o.OrderDate);
        revenueReport.EndDate = orders.Max(o => o.OrderDate);
        revenueReport.Revenue = orders.Sum(o => o.TotalPrice.NetPrice);

        return revenueReport;
    }
}