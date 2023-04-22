using Fulfillment.Application.Dtos;
using Fulfillment.Application.Services.Revenue;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fulfillment.Web;

[ApiController]
[Authorize(Roles = "Admin")]
[Route("api/[controller]")]
[Produces("application/json")]
public class RevenuesController : BaseController
{
    private readonly IRevenueService _revenueService;
    
    public RevenuesController(IRevenueService revenueService)
    {
        _revenueService = revenueService;
    }
    
    /// <summary>
    /// Returns a revenue report from a specified date range
    /// </summary>
    /// <param name="query">The date range parameter</param>
    /// <returns>A revenue report</returns>
    /// <response code="200">Revenue report was generated successfully</response>
    /// <response code="401">If the user is not authorized</response>
    /// <response code="403">If the user is not an admin</response>
    /// <response code="500">If an internal server error occurs</response>
    [HttpGet()]
    [ProducesResponseType(typeof(RevenueReportResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
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