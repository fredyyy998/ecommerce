using Account.Application.Dtos;
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
        var customer = _profileService.GetProfile(customerId);
        return Ok(customer);
    }
    
    [HttpPut("{customerId:guid}")]
    public IActionResult UpdateProfile(Guid customerId, CustomerUpdateDto customerUpdateDto)
    {
        _profileService.UpdateProfile(customerId, customerUpdateDto);
        return Ok();
    }
    
    [HttpDelete("{customerId:guid}")]
    public IActionResult DeleteProfile(Guid customerId)
    {
        _profileService.DeleteProfile(customerId);
        return Ok();
    }
}