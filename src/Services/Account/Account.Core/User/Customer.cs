using System.Dynamic;

namespace Account.Core.User;

public class Customer : User
{
    public Address Address { get; protected set; }
    
    public PersonalInformation PersonalInformation { get; protected set; }
    
    public PaymentInformation PaymentInformation { get; protected set; }

    public static Customer Create(string email, string password)
    {
        return new Customer
        {
            Id = Guid.NewGuid(),
            Email = email,
            Password = Password.Create(password),
            CreatedAt = DateTime.Now,
        };
    }

    public void UpdatePersonalInformation(string FirstName, string LastName, string DateOfBirth)
    {
        PersonalInformation = new PersonalInformation(FirstName, LastName, DateOnly.Parse(DateOfBirth));
        UpdatedAt = DateTime.Now;
    }

    public void UpdateAddress(string street, string zip, string city, string country)
    {
        Address = new Address(street, zip, city, country);
        UpdatedAt = DateTime.Now;
    }


    public void UpdatePaymentInformation(string street, string zip, string city, string country)
    {
        var paymentAddress = new Address(street, zip, city, country);
        PaymentInformation = new PaymentInformation(paymentAddress);
        UpdatedAt = DateTime.Now;
    }
}