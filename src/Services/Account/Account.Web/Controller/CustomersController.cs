using Account.Application.Dtos;
using Account.Application.Exceptions;
using Account.Application.Profile;
using Microsoft.AspNetCore.Mvc;

namespace Account.Web;

[ApiController]
[Route("/api/[controller]")]
public class CustomersController : Controller
{
    private readonly IProfileService _profileService;
    
    public CustomersController(IProfileService profileService)
    {
        _profileService = profileService;
    }
    
    [HttpGet("{customerId:guid}")]
    public IActionResult GetProfile(Guid customerId)
    {
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
    
    [HttpPut("{customerId:guid}")]
    public IActionResult UpdateProfile(Guid customerId, CustomerUpdateDto customerUpdateDto)
    {
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
    
    [HttpDelete("{customerId:guid}")]
    public IActionResult DeleteProfile(Guid customerId)
    {
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
}