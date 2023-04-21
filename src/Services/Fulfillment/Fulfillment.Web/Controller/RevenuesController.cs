using Fulfillment.Application.Dtos;
using Fulfillment.Application.Services.Revenue;
using Microsoft.AspNetCore.Mvc;

namespace Fulfillment.Web;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class RevenuesController : BaseController
{
    private readonly IRevenueService _revenueService;
    
    public RevenuesController(IRevenueService revenueService)
    {
        _revenueService = revenueService;
    }
    
    [HttpGet()]
    public async Task<IActionResult> GetRevenues([FromQuery] RevenueQuery query)
    {
        try
        {
            var revenues = await _revenueService.GetRevenueReportAsync(query);
            return Ok(revenues);
        }
        catch (Exception e)
        {
            return HandleException(e);
        }

    }
}