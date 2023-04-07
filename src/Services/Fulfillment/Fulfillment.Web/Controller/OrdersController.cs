using System.Security.Claims;
using Fulfillment.Application.Exceptions;
using Fulfillment.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fulfillment.Web;

[ApiController]
[Authorize]
[Route("/api/[controller]")]
public class OrdersController : Controller
{
    private readonly IOrderService _orderService;
    
    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrder(Guid id)
    {
        try
        {
            var order = await _orderService.GetOrder(id);
            return Ok(order);
        }
        catch (Exception e)
        {
            return HandleException(e);
        }
    }
    
    [HttpGet()]
    public async Task<IActionResult> GetOrders()
    {
        try
        {
            var buyerId = GetGuidFromClaims();
            var orders = await _orderService.GetOrdersByBuyer(buyerId);
            return Ok(orders);
        }
        catch (Exception e)
        {
            return HandleException(e);
        }
    }
    
    
    [HttpPut("{id}/state/cancel")]
    public async Task<IActionResult> SubmitOrder(Guid id)
    {
        try
        {
            await _orderService.CancelOrder(id);
            return Ok();
        }
        catch (Exception e)
        {
            return HandleException(e);
        }
    }
    
    
    private IActionResult HandleException(Exception exception)
    {
        if (exception is EntityNotFoundException)
        {
            return NotFound(exception.Message);
        }
        return BadRequest(exception.Message);
    }
    
    private Guid GetGuidFromClaims()
    {
        var buyerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(buyerId.ToString()))
        {
            throw new UnauthorizedException("User not authorized.");
        }
        return Guid.Parse(buyerId);
    }
}