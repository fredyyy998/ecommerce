using Account.Core.Events;
using Account.Core.User;

namespace Account.Test.User;

public class CustomerTest
{
    [Fact]
    public void CustomersPersonalInformationIsAdded()
    {
        var customer = CreateCustomer();

        customer.UpdatePersonalInformation("John", "Doe", "01.01.2000");
        
        Assert.Equal("John", customer.PersonalInformation.FirstName);
        Assert.Equal("Doe", customer.PersonalInformation.LastName);
        Assert.Equal("01.01.2000", customer.PersonalInformation.DateOfBirth.ToString("dd.MM.yyyy"));
    }

    [Fact]
    public void CustomersAddressIsAdded()
    {
        var customer = CreateCustomer();

        customer.UpdateAddress("Teststreet 1", "12345", "Testcity", "TestCountry");

        Assert.Equal("Teststreet 1", customer.Address.Street);
        Assert.Equal("12345", customer.Address.Zip);
        Assert.Equal("Testcity", customer.Address.City);
        Assert.Equal("TestCountry", customer.Address.Country);
    }

    [Fact]
    public void CustomersPaymentInformationIsAdded()
    {
        var customer = CreateCustomer();
        
        customer.UpdatePaymentInformation("Teststreet 1", "12345", "Testcity", "TestCountry");
        
        Assert.Equal("Teststreet 1", customer.PaymentInformation.Address.Street);
        Assert.Equal("12345", customer.PaymentInformation.Address.Zip);
        Assert.Equal("Testcity", customer.PaymentInformation.Address.City);
        Assert.Equal("TestCountry", customer.PaymentInformation.Address.Country);
    }
    
    [Fact]
    public void CustomerRegistrationEventIsAdded()
    {
        var customer = CreateCustomer();
        
        Assert.Single(customer.DomainEvents);
        Assert.IsType<CustomerRegisteredEvent>(customer.DomainEvents.First());
    }

    [Fact]
    public void CustomerEditedEvent_is_added_on_customer_changes()
    {
        var customer = CreateCustomer();
        customer.ClearEvents(); // Clear the registration event
        
        customer.UpdatePersonalInformation("John", "Doe", "01.01.2000");
        
        Assert.Single(customer.DomainEvents);
        Assert.IsType<CustomerEditedEvent>(customer.DomainEvents.First());
    }

    private Customer CreateCustomer()
    {
        return Customer.Create("user@test.de", "abc123");
    }
}