using Account.Application;
using Account.Application.Dtos;
using Account.Application.Exceptions;
using Account.Application.Profile;
using Account.Core.User;
using AutoMapper;
using Moq;

namespace Account.Test.Profile;

public class ProfileServiceTest
{
    
    private readonly Mock<ICustomerRepository> _customerRepository;
    private readonly IMapper _mapper;
    private readonly IProfileService _profileService;
    
    public ProfileServiceTest()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        _mapper = config.CreateMapper();
        
        _customerRepository = new Mock<ICustomerRepository>();
        _profileService = new ProfileService(_customerRepository.Object, _mapper);
    }
    
    [Fact]
    public async Task Customer_can_view_their_profile()
    {
        var customer = Customer.Create("customer@test.de", "abc123");
        _customerRepository.Setup(x => x.GetById(customer.Id))
            .Returns(Task.FromResult(customer));
        
        var profile = await _profileService.GetProfile(customer.Id);
        
        Assert.Equal(customer.Id, profile.Id);
        Assert.Equal(customer.Email, profile.Email);
    }

    [Fact]
    public void Customer_gets_EntityNotFoundException_when_customer_does_not_exist()
    {
        var notPersistedCustomer = Customer.Create("customer@test.de", "abc123");
        _customerRepository.Setup(x => x.GetById(It.IsAny<Guid>()))
            .ReturnsAsync((Customer) null);

        
        Assert.ThrowsAsync<EntityNotFoundException>(() => _profileService.GetProfile(notPersistedCustomer.Id));
    }

    [Fact]
    public void Customer_can_update_their_profile()
    {
        var customer = Customer.Create("customer@test.de", "abc123");
        var updateAddress = new AddressDto("street", "city", "zip", "country");
        var updatePersonalInfo = new PersonalInformationDto("firstName", "lastName", "01.01.2000");
        var paymentInformation = new PaymentInformationDto(new AddressDto("street", "city", "zip", "country"));
        var updateDto = new CustomerUpdateDto(updateAddress, updatePersonalInfo, paymentInformation);
        _customerRepository.Setup(x => x.GetById(customer.Id))
            .ReturnsAsync(customer);

        _profileService.UpdateProfile(customer.Id, updateDto);

        _customerRepository.Verify(x => x.Update(It.IsAny<Customer>()), Times.Once);
    }
    
    [Fact]
    public void Customer_can_update_their_profile_without_payment_information()
    {
        var customer = Customer.Create("customer@test.de", "abc123");
        var updateAddress = new AddressDto("street", "city", "zip", "country");
        var updatePersonalInfo = new PersonalInformationDto("firstName", "lastName", "01.01.2000");
        var updateDto = new CustomerUpdateDto(updateAddress, updatePersonalInfo, null);
        _customerRepository.Setup(x => x.GetById(customer.Id))
            .ReturnsAsync(customer);

        _profileService.UpdateProfile(customer.Id, updateDto);

        _customerRepository.Verify(x => x.Update(It.IsAny<Customer>()), Times.Once);
    }

    [Fact]
    public void Customer_can_delete_their_profile()
    {
        var customer = Customer.Create("customer@test.de", "avce123");
        _customerRepository.Setup(x => x.GetById(customer.Id))
            .ReturnsAsync(customer);
        _customerRepository.Setup(x => x.Delete(customer.Id));
        
        _profileService.DeleteProfile(customer.Id);
        
        _customerRepository.Verify(x => x.Delete(customer.Id), Times.Once);
    }

}