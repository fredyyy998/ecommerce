using System.Security.Claims;
using System.Text;
using Account.Application.Dtos;
using Account.Application.Exceptions;
using Account.Core.User;
using AutoMapper;
using FluentValidation;
using System.IdentityModel.Tokens.Jwt;
using Account.Core.Administrator;
using Microsoft.IdentityModel.Tokens;

namespace Account.Application.Profile;

public class AuthenticationService : IAuthenticationService
{
    private readonly ICustomerRepository _customerRepository;

    private readonly IAdministratorRepository _administratorRepository;

    private readonly IMapper _mapper;
    
    private readonly IValidator<CustomerCreateDto> _customerValidator;
    
    private readonly JwtInformation _jwtInformation;

    public AuthenticationService(
        ICustomerRepository customerRepository,
        IAdministratorRepository administratorRepository,
        IMapper mapper,
        IValidator<CustomerCreateDto> customerValidator,
        JwtInformation jwtInformation)
    {
        _customerRepository = customerRepository;
        _administratorRepository = administratorRepository;
        _mapper = mapper;
        _customerValidator = customerValidator;
        _jwtInformation = jwtInformation;
    }
    
    public async Task<CustomerResponseDto> RegisterCustomer(CustomerCreateDto userDto)
    {
        if (await _customerRepository.EmailExists(userDto.Email))
        {
            throw new EmailAlreadyExistsException(userDto.Email);
        }

        var validationResult = _customerValidator.Validate(userDto);
        if (validationResult.IsValid == false)
        {
            throw new ValidationException("Invalid data");
        }
        
        var user = Customer.Create(userDto.Email, userDto.Password);
        user = await _customerRepository.Create(user);
        return _mapper.Map<CustomerResponseDto>(user);
    }
    
    public async Task<JwtResponseDto> AuthenticateUser(string email, string password)
    {
        User user = await _customerRepository.GetByEmail(email);
        if (await _administratorRepository.EmailExists(email))
        {
            user = await _administratorRepository.GetAdministrator(email);
        }
        if (user is null ||user.Password.Verify(password) == false)
        {
            throw new InvalidLoginException(email);
        }

        var token = GenerateJwt(user);
        return new JwtResponseDto(token);
    }
    
    private string GenerateJwt(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtInformation.SecretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        };

        if (user is Administrator)
        {
            var admin = (Administrator) user;
            claims.Add(new Claim(ClaimTypes.Role, admin.Role));
        }

        var token = new JwtSecurityToken(
            issuer: _jwtInformation.Issuer,
            audience: _jwtInformation.Audience,
            claims: claims,
            expires: DateTime.Now.AddDays(7),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

