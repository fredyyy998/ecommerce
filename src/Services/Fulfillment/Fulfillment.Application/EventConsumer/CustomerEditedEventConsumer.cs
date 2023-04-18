using Fulfillment.Core.Buyer;
using Fulfillment.Core.DomainEvents;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Fulfillment.Application.EventConsumer;

public class CustomerEditedEventConsumer : INotificationHandler<CustomerEditedEvent>
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<CustomerEditedEventConsumer> _logger;

    public CustomerEditedEventConsumer(IServiceScopeFactory serviceScopeFactory, ILogger<CustomerEditedEventConsumer> logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
    }

    public async Task Handle(CustomerEditedEvent notification, CancellationToken cancellationToken)
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            try
            {
                var customerRepository = scope.ServiceProvider.GetRequiredService<IBuyerRepository>();
                var customer = await customerRepository.FindByIdAsync(notification.CustomerId);
                customer.UpdatePersonalInformation(notification.PersonalInformation.FirstName, notification.PersonalInformation.LastName, notification.Email);
                customer.UpdateShippingAddress(notification.Address.Street, notification.Address.ZipCode, notification.Address.City, notification.Address.Country);
                customer.UpdatePaymentInformation(notification.PaymentInformation.Address.Street, notification.PaymentInformation.Address.ZipCode, notification.PaymentInformation.Address.City, notification.PaymentInformation.Address.Country);
                await customerRepository.UpdateAsync(customer);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while handling CustomerEditedEvent");
            }

        }
    }
}