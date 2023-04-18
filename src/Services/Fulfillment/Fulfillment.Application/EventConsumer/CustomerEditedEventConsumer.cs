using Fulfillment.Core.Buyer;
using Fulfillment.Core.DomainEvents;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Fulfillment.Application.EventConsumer;

public class CustomerEditedEventConsumer : INotificationHandler<CustomerEditedEvent>
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    
    public CustomerEditedEventConsumer(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }
    
    public async Task Handle(CustomerEditedEvent notification, CancellationToken cancellationToken)
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var customerRepository = scope.ServiceProvider.GetRequiredService<IBuyerRepository>();
            var customer = await customerRepository.FindByIdAsync(notification.CustomerId);
            customer.UpdatePersonalInformation(notification.PersonalInformation.FirstName, notification.PersonalInformation.LastName, notification.Email);
            customer.UpdateShippingAddress(notification.Address.Street, notification.Address.ZipCode, notification.Address.City, notification.Address.Country);
            customer.UpdatePaymentInformation(notification.PaymentInformation.Address.Street, notification.PaymentInformation.Address.ZipCode, notification.PaymentInformation.Address.City, notification.PaymentInformation.Address.Country);
            customerRepository.UpdateAsync(customer);
        }
    }
}