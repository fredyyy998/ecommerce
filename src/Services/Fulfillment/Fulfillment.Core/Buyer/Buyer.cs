using Ecommerce.Common.Core;

namespace Fulfillment.Core.Buyer;

public class Buyer : EntityRoot
{
    public PersonalInformation PersonalInformation { get; set; }
    Address ShippingAddress { get; set; }
    PaymentInformation PaymentInformation { get; set; }
    
    public Buyer(string firstName, string lastName, string email, Address shippingAddress, PaymentInformation paymentInformation)
    {
        Id = Guid.NewGuid();
        PersonalInformation = new PersonalInformation(firstName, lastName, email);
        ShippingAddress = shippingAddress;
        PaymentInformation = paymentInformation;
    }
    
    public void UpdatePersonalInformation(string firstName, string lastName, string email)
    {
        PersonalInformation = new PersonalInformation(firstName, lastName, email);
    }
    
    public void UpdateShippingAddress(string street, string zip, string city, string country)
    {
        ShippingAddress = new Address(street, zip, city, country);
    }

    public void UpdatePaymentInformation(string street, string zip, string city, string country)
    {
        var paymentAddress = new Address(street, zip, city, country);
        PaymentInformation = new PaymentInformation(paymentAddress);
    }

}
