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
public class CustomersController : Controller
{
    private readonly IProfileService _profileService;
    
    public CustomersController(IProfileService profileService)
    {
        _profileService = profileService;
    }
    
    [HttpGet()]
    public IActionResult GetProfile()
    {
        var customerId = GetGuidFromClaims();
        try
        {
            var customer = _profileService.GetProfile(customerId);
            return Ok(customer);
        }
        catch (EntityNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
    
    [HttpPut()]
    public IActionResult UpdateProfile(CustomerUpdateDto customerUpdateDto)
    {
        var customerId = GetGuidFromClaims();
        try
        {
            _profileService.UpdateProfile(customerId, customerUpdateDto);
            return Ok();
        }
        catch (EntityNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
    
    [HttpDelete()]
    public IActionResult DeleteProfile()
    {
         var customerId = GetGuidFromClaims();
        try
        {
            _profileService.DeleteProfile(customerId);
            return Ok();
        }
        catch (EntityNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
    
    private Guid GetGuidFromClaims()
    {
        var claims = User.Claims;
        var customerId= claims.FirstOrDefault(x=>x.Type==ClaimTypes.NameIdentifier).Value;
        return Guid.Parse(customerId);
    }
}