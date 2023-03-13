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
public class CustomerProfilesController : Controller
{
    private readonly IProfileService _profileService;
    
    public CustomerProfilesController(IProfileService profileService)
    {
        _profileService = profileService;
    }
    
    [HttpGet()]
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
    
    [HttpPut()]
    public IActionResult UpdateProfile(CustomerUpdateDto customerUpdateDto)
    {
        try
        {
            var customerId = GetGuidFromClaims();
            _profileService.UpdateProfile(customerId, customerUpdateDto);
            return Ok();
        }
        catch (Exception e)
        {
            return HandleException(e);
        }
    }
    
    [HttpDelete()]
    public IActionResult DeleteProfile()
    {
        try
        {
            var customerId = GetGuidFromClaims();
            _profileService.DeleteProfile(customerId);
            return Ok();
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