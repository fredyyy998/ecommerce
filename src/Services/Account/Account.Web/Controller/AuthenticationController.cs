using Account.Application.Dtos;
using Account.Application.Exceptions;
using Account.Application.Profile;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Account.Web;

/// <summary>
/// Authentication for the application users
/// </summary>
[ApiController]
[Route("/api/[controller]")]
[Produces("application/json")]
public class AuthenticationController : Controller
{
    private IAuthenticationService _authenticationService;
    
    public AuthenticationController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }
    
    /// <summary>
    /// Registers a new customer
    /// </summary>
    /// <param name="customerDto">The email and the clear text password for the customer.</param>
    /// <returns>A newly generated customer</returns>
    /// <remarks>
    /// Sample request:
    /// 
    ///      Post /api/authentication/register
    ///     {
    ///         "email": "maxmusterman@mail.com",
    ///         "password": "abc123"
    ///     }
    /// 
    /// </remarks>
    /// <response code="200">Returns the newly created customer</response>
    /// <response code="400">If the email already exists or the data is invalid</response>
    [HttpPost]
    [Route("register")]
    [ProducesResponseType(typeof(CustomerResponseDto),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string),StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RegisterCustomer(CustomerCreateDto customerDto)
    {
        try
        {
            var customer = await _authenticationService.RegisterCustomer(customerDto);
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
    
    /// <summary>
    /// Logs a user into his account
    /// </summary>
    /// <param name="email">The email of the customer</param>
    /// <param name="password">The clear text password of the customer</param>
    /// <returns>A jwt token</returns>
    ///  <remarks>
    /// Sample request:
    ///
    ///     Post /api/authentication/login
    ///     {
    ///         "email": "maxmusterman@mail.com,
    ///         "password": "abc123"
    ///     }
    ///
    /// </remarks>
    /// <response code="200">Returns the jwt token</response>
    /// <response code="400">If anything is invalid, for anonymous reasons</response>
    [HttpPost]
    [Route("login")]
    [ProducesResponseType(typeof(JwtResponseDto),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string),StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> LoginCustomer(string email, string password)
    {
        try
        {
            var jwtResponse = await _authenticationService.AuthenticateUser(email, password);
            return Ok(jwtResponse);
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