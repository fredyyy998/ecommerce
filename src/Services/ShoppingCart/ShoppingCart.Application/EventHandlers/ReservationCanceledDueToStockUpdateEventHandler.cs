using MediatR;
using Microsoft.Extensions.Logging;
using ShoppingCart.Core.Events;
using ShoppingCart.Core.ShoppingCart;

namespace ShoppingCart.Application.EventConsumer;

public class ReservationCanceledDueToStockUpdateEventHandler : INotificationHandler<ReservationCanceledDueToStockUpdateEvent>
{
    private readonly IShoppingCartRepository _shoppingCartRepository;
    private readonly ILogger<ReservationCanceledDueToStockUpdateEventHandler> _logger;
    
    public ReservationCanceledDueToStockUpdateEventHandler(IShoppingCartRepository shoppingCartRepository, ILogger<ReservationCanceledDueToStockUpdateEventHandler> logger)
    {
        _shoppingCartRepository = shoppingCartRepository;
        _logger = logger;
    }
    
    public Task Handle(ReservationCanceledDueToStockUpdateEvent notification, CancellationToken cancellationToken)
    {
        try
        {
            _shoppingCartRepository.RemoveProductFromShoppingCart(notification.ShoppingCartId, notification.ProductId);
            _logger.LogInformation($"Removed product {notification.ProductId} from shopping cart {notification.ShoppingCartId} due to event");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while removing product from shopping cart");
        }
        
        return Task.CompletedTask;
    }
}