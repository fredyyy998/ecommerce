using Fulfillment.Core.Buyer;
using Fulfillment.Core.DomainEvents;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Fulfillment.Application.EventConsumer;

public class CustomerRegisteredEventConsumer : INotificationHandler<CustomerRegisteredEvent>
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<CustomerRegisteredEventConsumer> _logger;

    public CustomerRegisteredEventConsumer(IServiceScopeFactory serviceScopeFactory, ILogger<CustomerRegisteredEventConsumer> logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
    }
    
    public async Task Handle(CustomerRegisteredEvent notification, CancellationToken cancellationToken)
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            try
            {
                var customerRepository = scope.ServiceProvider.GetRequiredService<IBuyerRepository>();
                var customer = Buyer.Create(
                    id: notification.CustomerId,
                    personalInformation: new PersonalInformation(notification.Email)); 
                await customerRepository.SaveAsync(customer);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error while handling CustomerRegisteredEvent");
            }

        }
    }
}