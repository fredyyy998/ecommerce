using Account.Application;
using Account.Application.Dtos;
using Account.Application.Exceptions;
using Account.Application.Profile;
using Account.Core.Administrator;
using Account.Core.User;
using AutoMapper;
using FluentValidation;
using Moq;

namespace Account.Test.Profile;

public class AuthenticationTests
{
    // outer dependencies - mock implementations - other project or other systems
    private Mock<ICustomerRepository> _customerRepository;
    private Mock<IMapper> _mapper;
    private Mock<IAdministratorRepository> _administratorRepository;

    // inner dependencies - concrete implementations
    private IValidator<CustomerCreateDto> _customerValidator;
    private IAuthenticationService _authenticationService;

    public AuthenticationTests()
    {
        _customerRepository = new Mock<ICustomerRepository>();
        _administratorRepository = new Mock<IAdministratorRepository>();
        _mapper = new Mock<IMapper>();
        _customerValidator = new CustomerCreateDtoValidator();
        var jwtInformation = new JwtInformation("0Ukke8V63dDaWqgX0Ukke8V63dDaWqgX", "testIssuer", "testAudience");
        _authenticationService = new AuthenticationService(_customerRepository.Object, _administratorRepository.Object, _mapper.Object, _customerValidator, jwtInformation);
    }


    [Fact]
    public void Register_Customer_Stores_Customer()
    {
        var customer = new CustomerCreateDto("customer@test.de", "abc123");

        _authenticationService.RegisterCustomer(customer);
        
        _customerRepository.Verify(repository => repository.Create(It.IsAny<Customer>()), Times.Once);
    }

    [Fact]
    public void Register_Throws_Exception_On_Duplicated_Email()
    {
        var customer = new CustomerCreateDto("customer@test.de", "abc123");
        var customer2 = new CustomerCreateDto("customer@test.de", "123abc");
        _customerRepository.SetupSequence(x => x.EmailExists(customer.Email))
            .ReturnsAsync(false)
            .ReturnsAsync(true);


        _authenticationService.RegisterCustomer(customer);

        Assert.ThrowsAsync<EmailAlreadyExistsException>(() => _authenticationService.RegisterCustomer(customer2));
    }

    [Fact]
    public void Register_Throws_Exception_On_Invalid_Email()
    {
        var customer = new CustomerCreateDto("invalidMail", "abc123");

        Assert.ThrowsAsync<ValidationException>(() => _authenticationService.RegisterCustomer(customer));
    }

    [Fact]
    public void Register_Throws_Exception_On_Short_Password()
    {
        var customer = new CustomerCreateDto("test@test.de", "abc");

        Assert.ThrowsAsync<ValidationException>(() => _authenticationService.RegisterCustomer(customer));
    }

    [Fact]
    public void Login_is_not_valid_if_user_does_not_exist()
    {
        var email = "test@test.de";
        var password = "abc123";
        var customer = Customer.Create(email, password);
        _customerRepository.Setup(x => x.GetByEmail(customer.Email))
            .ReturnsAsync((Customer) null);
        
        Assert.ThrowsAsync<InvalidLoginException>(() => _authenticationService.AuthenticateUser(email, password));
    }
    
    [Fact]
    public void Login_is_not_valid_on_non_matching_password()
    {
        var email = "test@test.de";
        var password = "abc123";
        var customer = Customer.Create(email, password);
        _customerRepository.Setup(x => x.GetByEmail(customer.Email))
            .ReturnsAsync(customer);
        
        Assert.ThrowsAsync<InvalidLoginException>(() => _authenticationService.AuthenticateUser(email, "abc12345"));
    }
}