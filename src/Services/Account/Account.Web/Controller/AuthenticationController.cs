using Account.Application.Dtos;
using Account.Application.Exceptions;
using Account.Application.Profile;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Account.Web;

[ApiController]
[Route("/api/[controller]")]
public class AuthenticationController : Controller
{
    private IAuthenticationService _authenticationService;
    
    public AuthenticationController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }
    
    [HttpPost]
    [Route("register")]
    public IActionResult RegisterCustomer(CustomerCreateDto customerDto)
    {
        try
        {
            var customer = _authenticationService.RegisterCustomer(customerDto);
            return Ok(customer);
            // TODO better would be following response, but the routing is not working:
            // CreatedAtAction(
            //     nameof(RegisterCustomer),
            //     new { id = customer.Id },
            //     customer);
        }
        catch (EmailAlreadyExistsException e)
        {
            return BadRequest("Email already exists");
        }
        catch (ValidationException e)
        {
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
    
    [HttpPost]
    [Route("login")]
    public IActionResult LoginCustomer(string email, string password)
    {
        try
        {
            var jwtToken = _authenticationService.AuthenticateCustomer(email, password);
            return Ok(jwtToken);
        }
        catch (InvalidLoginException e)
        {
            return BadRequest("Invalid login");
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}