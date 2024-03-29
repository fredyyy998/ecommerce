﻿using Ecommerce.Common.Core;
using ShoppingCart.Core.Events;
using ShoppingCart.Core.Exceptions;

namespace ShoppingCart.Core.ShoppingCart;

public class ShoppingCart : EntityRoot
{
    public Guid CustomerId { get; private set; }

    private List<ShoppingCartItem> _items;
    
    public IReadOnlyCollection<ShoppingCartItem> Items => _items.AsReadOnly();
    
    public State Status { get; private set; }
    
    public ShoppingCartCheckout ShoppingCartCheckout { get; private set; }
    
    public DateTime CreatedAt { get; private set; }
    
    public DateTime? UpdatedAt { get; private set; }
    
    private ShoppingCart(Guid customerId)
    {
        Id = Guid.NewGuid();
        CustomerId = customerId;
        _items = new List<ShoppingCartItem>();
        Status = State.Active;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = null;
    }
    
    public static ShoppingCart Create(Guid customerId)
    {
        return new ShoppingCart(customerId);
    }

    public void AddItem(Product.Product product, int quantity)
    {
        var item = _items.FirstOrDefault(x => x.Product.Id == product.Id);
        if (!product.HasSufficientStock(quantity))
        {
            throw new ShoppingCartDomainException("Not enough stock.");
        }
        if (item != null)
        {
            item.IncreaseQuantity(quantity);
            AddDomainEvent(new CustomerChangedProductQuantityInCartEvent(this.Id, product, item.Quantity));
        }
        else
        {
            item = ShoppingCartItem.Create(product, quantity);
            _items.Add(item);
            AddDomainEvent(new CustomerAddedProductToBasketEvent(Id, product, quantity));
        }
        product.Reservate(quantity, Id);
        UpdatedAt = DateTime.UtcNow;
    }

    public void RemoveQuantityOfItem(Product.Product product, int quantity)
    {
        var item = _items.FirstOrDefault(x => x.Product.Id == product.Id);
        if (item == null)
        {
            throw new ShoppingCartDomainException("Product not found in shopping cart.");
        }
        item.DecreaseQuantity(quantity);
        
        AddDomainEvent(new CustomerChangedProductQuantityInCartEvent(this.Id, product, item.Quantity));
        if (item.Quantity == 0)
        {
            _items.Remove(item);
        }
        else
        {
            product.Reservate(quantity, Id);
        }
        UpdatedAt = DateTime.UtcNow;
    }

    public void MarkAsTimedOut()
    {
        Status = State.TimedOut;
        foreach (var item in _items)
        {
            item.Product.CancelReservation(Id);
        }
        AddDomainEvent(new ShoppingCartTimedOutEvent(this));
    }

    public void RemoveItem(Product.Product product)
    {
        var item = _items.FirstOrDefault(x => x.Product.Id == product.Id);
        if (item == null)
        {
            throw new ShoppingCartDomainException("Product not found in shopping cart.");
        }
        RemoveQuantityOfItem(product, item.Quantity);
    }

    public void Checkout(ShoppingCartCheckout shoppingCartCheckout)
    {
        ShoppingCartCheckout = shoppingCartCheckout;
        MarkAsOrdered();
    }
    
    private void MarkAsOrdered()
    {
        Status = State.Ordered;
        foreach (var item in _items)
        {
            item.Product.CommitReservation(Id);
        }
        AddDomainEvent(new CustomerOrderedShoppingCartEvent(this));
    }
}