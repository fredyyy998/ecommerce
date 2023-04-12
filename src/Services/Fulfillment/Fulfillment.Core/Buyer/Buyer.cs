using Ecommerce.Common.Core;

namespace Fulfillment.Core.Buyer;

public class Buyer : EntityRoot

{
    public PersonalInformation PersonalInformation { get; set; }
    public Address ShippingAddress { get; set; }
    public PaymentInformation PaymentInformation { get; set; }
    
    public static Buyer Create(Guid id, PersonalInformation personalInformation)
    {
        return new Buyer()
        {
            Id = id,
            PersonalInformation = personalInformation,
        };
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
