using Fulfillment.Core.Buyer;
using Fulfillment.Core.DomainEvents;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Fulfillment.Application.EventConsumer;

public class CustomerRegisteredEventConsumer : INotificationHandler<CustomerRegisteredEvent>
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    
    public CustomerRegisteredEventConsumer(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }
    
    public async Task Handle(CustomerRegisteredEvent notification, CancellationToken cancellationToken)
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var customerRepository = scope.ServiceProvider.GetRequiredService<IBuyerRepository>();
            var customer = Buyer.Create(
                id: notification.CustomerId,
                personalInformation: new PersonalInformation(notification.Email)); 
            customerRepository.SaveAsync(customer);
        }
    }
}