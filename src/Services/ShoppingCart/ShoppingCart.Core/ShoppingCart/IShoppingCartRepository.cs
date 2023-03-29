﻿using System.Collections.ObjectModel;
using Ecommerce.Common.Core;

namespace ShoppingCart.Core.ShoppingCart;

public interface IShoppingCartRepository : IRepository<ShoppingCart>
{
    List<ShoppingCart> GetTimedOutShoppingCarts();
    List<ShoppingCart> GetShoppingCartsByCustomerId(Guid customerId);
    ShoppingCart GetActiveShoppingCartByCustomer(Guid customerId);
}