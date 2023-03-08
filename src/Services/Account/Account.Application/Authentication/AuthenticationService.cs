using System.Security.Claims;
using System.Text;
using Account.Application.Dtos;
using Account.Application.Exceptions;
using Account.Core.User;
using AutoMapper;
using FluentValidation;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Account.Application.Profile;

public class AuthenticationService : IAuthenticationService
{
    private readonly ICustomerRepository _customerRepository;

    private readonly IMapper _mapper;
    
    private readonly IValidator<CustomerCreateDto> _customerValidator;
    
    private readonly JwtInformation _jwtInformation;

    public AuthenticationService(
        ICustomerRepository customerRepository,
        IMapper mapper,
        IValidator<CustomerCreateDto> customerValidator,
        JwtInformation jwtInformation)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
        _customerValidator = customerValidator;
        _jwtInformation = jwtInformation;
    }
    
    public CustomerResponseDto RegisterCustomer(CustomerCreateDto userDto)
    {
        if (_customerRepository.EmailExists(userDto.Email))
        {
            throw new EmailAlreadyExistsException(userDto.Email);
        }

        var validationResult = _customerValidator.Validate(userDto);
        if (validationResult.IsValid == false)
        {
            throw new ValidationException("Invalid data");
        }
        
        var user = Customer.Create(userDto.Email, userDto.Password);
        _customerRepository.Create(user);
        return _mapper.Map<CustomerResponseDto>(user);
    }
    
    public string AuthenticateCustomer(string email, string password)
    {
        var user = _customerRepository.GetByEmail(email);
        if (user is null ||user.Password.Verify(password) == false)
        {
            throw new InvalidLoginException(email);
        }

        return GenerateToken(user);
    }
    
    private string GenerateToken(Customer customer)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtInformation.SecretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.Email, customer.Email),
            new Claim(ClaimTypes.NameIdentifier, customer.Id.ToString()),
        };

        var token = new JwtSecurityToken(
            issuer: _jwtInformation.Issuer,
            audience: _jwtInformation.Audience,
            claims: claims,
            expires: DateTime.Now.AddDays(7),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    
    public ClaimsPrincipal ValidateToken(string token)
    {
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtInformation.SecretKey)),
            ValidateIssuer = true,
            ValidIssuer = _jwtInformation.Issuer,
            ValidateAudience = true,
            ValidAudience = _jwtInformation.Audience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };

        var handler = new JwtSecurityTokenHandler();
        try
        {
            var claimsPrincipal = handler.ValidateToken(token, validationParameters, out var securityToken);
            return claimsPrincipal;
        }
        catch (Exception)
        {
            // token is invalid
            return null;
        }
    }
}

