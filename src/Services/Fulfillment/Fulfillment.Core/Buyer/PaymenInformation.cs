﻿using Ecommerce.Common.Core;

namespace Fulfillment.Core.Buyer;

public class PaymentInformation : ValueObject
{ 
    public Address Address { get; protected set; }
    
    protected PaymentInformation() { }
    
    public PaymentInformation(Address address)
    {
        Address = address;
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Address;
    }
}