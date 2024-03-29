﻿using ShoppingCart.Application.Dtos;

namespace ShoppingCart.Application.Services;

public interface IShoppingCartService
{
    Task AddProductToShoppingCart(Guid customerId, Guid productId, int quantity);
    Task RemoveProductFromShoppingCart(Guid customerId, Guid productId);
    Task<ShoppingCartResponseDto> GetActiveShoppingCart(Guid customerId);
    Task Checkout(Guid customerId, CheckoutRequestDto checkoutRequestDto);
    Task TimeOutShoppingCarts();
}