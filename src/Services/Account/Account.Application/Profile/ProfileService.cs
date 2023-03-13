using System.Threading.Channels;
using Account.Application.Dtos;
using Account.Application.Exceptions;
using Account.Core.User;
using AutoMapper;

namespace Account.Application.Profile;

public class ProfileService : IProfileService
{
    
    private readonly ICustomerRepository _customerRepository;

    private readonly IMapper _mapper;
    
    public ProfileService(ICustomerRepository customerRepository, IMapper mapper)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
    }
    
    public CustomerResponseDto GetProfile(Guid customerId)
    {
        var customer = GetExistingCustomerById(customerId);
        return _mapper.Map<CustomerResponseDto>(customer);
    }



    public void UpdateProfile(Guid customerId, CustomerUpdateDto customerUpdateDto)
    {
        var customer = GetExistingCustomerById(customerId);
        
        customer.UpdateAddress(customerUpdateDto.Address.Street, customerUpdateDto.Address.City, customerUpdateDto.Address.Zip, customerUpdateDto.Address.Country);
        customer.UpdatePersonalInformation(customerUpdateDto.PersonalInformation.FirstName, customerUpdateDto.PersonalInformation.LastName, customerUpdateDto.PersonalInformation.DateOfBirth);

        if (customerUpdateDto.PaymentInformation is not null)
        {
            customer.UpdatePaymentInformation(customerUpdateDto.PaymentInformation.Address.Street, customerUpdateDto.PaymentInformation.Address.City, customerUpdateDto.PaymentInformation.Address.Zip, customerUpdateDto.PaymentInformation.Address.Country);
        }
        
        _customerRepository.Update(customer);
    }

    public void DeleteProfile(Guid customerId)
    {
        var customer = GetExistingCustomerById(customerId);
        _customerRepository.Delete(customerId);
    }
    
    private Customer GetExistingCustomerById(Guid customerId)
    {
        var customer = _customerRepository.GetById(customerId);
        if (customer is null)
        {
            throw new EntityNotFoundException("Customer not found");
        }

        return customer;
    }
}