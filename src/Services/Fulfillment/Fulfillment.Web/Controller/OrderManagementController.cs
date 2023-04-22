using Ecommerce.Common.Core;
using Fulfillment.Application.Dtos;
using Fulfillment.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fulfillment.Web;


[ApiController]
[Authorize(Roles = "Admin")]
[Route("/api/[controller]")]
[Produces("application/json")]
public class OrderManagementController : BaseController
{
    private readonly IOrderService _orderService;
    
    public OrderManagementController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    /// <summary>
    /// Changes the state of an order to Shipped
    /// Only usable by Admins
    /// </summary>
    /// <param name="orderId">The id of the order</param>
    /// <returns>nothing</returns>
    /// <response code="204">If the order was shipped successfully</response>
    /// <response code="401">If the user is not authorized</response>
    /// <response code="403">If the user is not an admin</response>
    /// <response code="404">If the order does not exist</response>
    /// <response code="500">If an internal server error occurs</response>
    [HttpPut("{orderId}/state/ship")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ShipOrder(Guid orderId)
    {
        try
        {
            await _orderService.ShipOrder(orderId);
            return NoContent();
        }
        catch (Exception e)
        {
            return HandleException(e);
        }
    }
    
    /// <summary>
    /// Returns all orders in a paginated list
    /// </summary>
    /// <param name="parameters">The pagination and filter parameters</param>
    /// <returns>A paginated list of orders</returns>
    /// <response code="200">Orders were found successfully</response>
    /// <response code="401">If the user is not authorized</response>
    /// <response code="403">If the user is not an admin</response>
    /// <response code="500">If an internal server error occurs</response>
    [HttpGet()]
    [ProducesResponseType(typeof(PagedList<OrderResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(string), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> FindAll([FromQuery] OrderAdminParameter parameters)
    {
        try
        {
            var result = await _orderService.FindReadyToShipAsync(parameters);
            return Ok(result.Item1);
        }
        catch (Exception e)
        {
            return HandleException(e);
        }
    }
}