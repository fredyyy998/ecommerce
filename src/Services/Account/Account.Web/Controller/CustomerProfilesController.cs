using System.Security.Claims;
using Account.Application.Dtos;
using Account.Application.Exceptions;
using Account.Application.Profile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Account.Web;

[ApiController]
[Authorize]
[Route("/api/[controller]")]
[Produces("application/json")]
public class CustomerProfilesController : Controller
{
    private readonly IProfileService _profileService;
    
    public CustomerProfilesController(IProfileService profileService)
    {
        _profileService = profileService;
    }
    
    /// <summary>
    /// Gets the profile of the current authenticated customer.
    /// </summary>
    /// <returns>The customer data</returns>
    /// <response code="200">Returns the customer data</response>
    /// <response code="401">If the user is not authorized</response>
    /// <response code="404">If the customer does not exist</response>
    /// <response code="500">If an internal server error occurs</response>
    [HttpGet()]
    [ProducesResponseType(typeof(CustomerResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public IActionResult GetProfile()
    {
        try
        {
            var customerId = GetGuidFromClaims();
            var customer = _profileService.GetProfile(customerId);
            return Ok(customer);
        }
        catch (Exception e)
        {
            return HandleException(e);
        }
    }
    
    /// <summary>
    /// Updates the profile of the current authenticated customer.
    /// </summary>
    /// <param name="customerUpdateDto">The profile data to update</param>
    /// <returns>nothing</returns>
    /// <response code="204">If the profile was updated successfully</response>
    /// <response code="401">If the user is not authorized</response>
    /// <response code="404">If the customer does not exist</response>
    /// <response code="500">If an internal server error occurs</response>
    [HttpPut()]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public IActionResult UpdateProfile(CustomerUpdateDto customerUpdateDto)
    {
        try
        {
            var customerId = GetGuidFromClaims();
            _profileService.UpdateProfile(customerId, customerUpdateDto);
            return NoContent();
        }
        catch (Exception e)
        {
            return HandleException(e);
        }
    }
    
    /// <summary>
    /// Deletes the complete user data of the current authenticated customer.
    /// </summary>
    /// <returns>nothing</returns>
    /// <response code="204">If the profile was deleted successfully</response>
    /// <response code="401">If the user is not authorized</response>
    /// <response code="404">If the customer does not exist</response>
    /// <response code="500">If an internal server error occurs</response>
    [HttpDelete()]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
    public IActionResult DeleteProfile()
    {
        try
        {
            var customerId = GetGuidFromClaims();
            _profileService.DeleteProfile(customerId);
            return NoContent();
        }
        catch (Exception e)
        {
            return HandleException(e);
        }
    }
    
    private Guid GetGuidFromClaims()
    {
        var customerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(customerId))
        {
            throw new UnauthorizedException("User not authorized.");
        }
        return Guid.Parse(customerId);
    }
    
    private IActionResult HandleException(Exception e)
    {
        if (e is EntityNotFoundException)
        {
            return NotFound(e.Message);
        }
        if (e is UnauthorizedException)
        {
            return Unauthorized(e.Message);
        }
        return StatusCode(StatusCodes.Status500InternalServerError);
    }
}