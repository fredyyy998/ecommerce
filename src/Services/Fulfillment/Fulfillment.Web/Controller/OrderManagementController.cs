using Fulfillment.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fulfillment.Web;

[ApiController]
[Authorize(Roles = "Admin")]
[Route("/api/[controller]")]
public class OrderManagementController : BaseController
{
    private readonly IOrderService _orderService;
    
    public OrderManagementController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPut("{id}/state/ship")]
    public async Task<IActionResult> ShipOrder(Guid id)
    {
        try
        {
            await _orderService.ShipOrder(id);
            return Ok();
        }
        catch (Exception e)
        {
            return HandleException(e);
        }
    }
}