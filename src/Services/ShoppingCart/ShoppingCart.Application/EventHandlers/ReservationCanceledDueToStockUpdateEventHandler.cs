using MediatR;
using ShoppingCart.Core.Events;
using ShoppingCart.Core.ShoppingCart;

namespace ShoppingCart.Application.EventConsumer;

public class ReservationCanceledDueToStockUpdateEventHandler : INotificationHandler<ReservationCanceledDueToStockUpdateEvent>
{
    private readonly IShoppingCartRepository _shoppingCartRepository;
    
    public ReservationCanceledDueToStockUpdateEventHandler(IShoppingCartRepository shoppingCartRepository)
    {
        _shoppingCartRepository = shoppingCartRepository;
    }
    
    public Task Handle(ReservationCanceledDueToStockUpdateEvent notification, CancellationToken cancellationToken)
    {
        _shoppingCartRepository.RemoveProductFromShoppingCart(notification.ShoppingCartId, notification.ProductId);
        return Task.CompletedTask;
    }
}