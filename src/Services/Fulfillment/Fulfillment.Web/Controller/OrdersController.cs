using System.Security.Claims;
using Fulfillment.Application.Dtos;
using Fulfillment.Application.Exceptions;
using Fulfillment.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fulfillment.Web;

[ApiController]
[Authorize]
[Route("/api/[controller]")]
[Produces("application/json")]
public class OrdersController : BaseController
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    /// <summary>
    /// Gets the order by id, when the order is by the current authenticated customer.
    /// </summary>
    /// <param name="orderId">The id of the order</param>
    /// <returns>The order object</returns>
    /// <response code="200">Returns the order object</response>
    /// <response code="401">If the user is not authorized</response>
    /// <response code="404">If the order does not exist</response>
    /// <response code="500">If an internal server error occurs</response>
    [HttpGet("{orderId}")]
    [ProducesResponseType(typeof(OrderResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetOrder(Guid orderId)
    {
        try
        {
            var order = await _orderService.GetOrder(orderId);
            return Ok(order);
        }
        catch (Exception e)
        {
            return HandleException(e);
        }
    }
    
    /// <summary>
    /// Gets the orders of the current authenticated customer.
    /// </summary>
    /// <returns>The orders as order object list</returns>
    /// <response code="200">Returns the orders as order object list</response>
    /// <response code="401">If the user is not authorized</response>
    /// <response code="500">If an internal server error occurs</response>
    [HttpGet()]
    [ProducesResponseType(typeof(List<OrderResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
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
    
    /// <summary>
    /// Cancels the order by id, when the order is by the current authenticated customer. 
    /// </summary>
    /// <param name="orderId">The id of the order to be canceled</param>
    /// <returns>nothing</returns>
    /// <response code="204">Returns nothing</response>
    /// <response code="401">If the user is not authorized</response>
    /// <response code="500">If an internal server error occurs</response>
    /// <response code="404">If the order does not exist</response>
    [HttpPut("{orderId}/state/cancel")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CancelOrder(Guid orderId)
    {
        try
        {
            await _orderService.CancelOrder(orderId);
            return NoContent();
        }
        catch (Exception e)
        {
            return HandleException(e);
        }
    }

    /// <summary>
    /// This is just a utility api for now until the payment is implemented accordingly
    /// The api change the state of the order to payed
    /// </summary>
    /// <param name="id">The guid of the order that changes the state to payed</param>
    /// <returns>Returns wether the request succeeded or not</returns>
    [HttpPut("{id}/state/pay")]
    public async Task<IActionResult> PayOrder(Guid id)
    {
        try
        {
            await _orderService.PayOrder(id);
            return NoContent();
        }
        catch (Exception e)
        {
            return HandleException(e);
        }
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