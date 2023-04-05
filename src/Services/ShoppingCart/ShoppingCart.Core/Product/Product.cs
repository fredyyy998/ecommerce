using Ecommerce.Common.Core;
using ShoppingCart.Core.Events;
using ShoppingCart.Core.Exceptions;

namespace ShoppingCart.Core.Product;

public class Product : EntityRoot
{
    public string Name { get; private set; }
    
    public string Description { get; private set; }
    
    public Price Price { get; private set; }
    
    private int _stock;
    public int Stock 
    { 
        get => _stock;
        private set
        {
            if (TotalReserved > value)
            {
                DumpReservationUntilQuantityThreshold(value);
            }
            _stock = value;
        } 
    }

    private List<Reservation> _reservations = new List<Reservation>();
    
    public int TotalReserved => _reservations.Sum(x => x.Quantity);
    
    public IReadOnlyCollection<Reservation> Reservations => _reservations.AsReadOnly();

    public static Product Create(Guid id, string name, string description, Price price, int stock)
    {
        if (stock < 0)
        {
            throw new ProductDomainException("Stock must be greater than zero");
        }
        
        return new Product
        {
            Id = id,
            Name = name,
            Description = description,
            Price = price,
            Stock = stock,
        };
    }
    
    public void Update(string name, string description, Price price, int stock)
    {
        if (stock < 0)
        {
            throw new ProductDomainException("Stock must be greater than zero");
        }
        
        Name = name;
        Description = description;
        Price = price;
        Stock = stock;
    }
    
    public bool HasSufficientStock(int quantity)
    {
        return Stock - quantity - TotalReserved >= 0;
    }

    public void RemoveStock(int quantity)
    {
        if (quantity <= 0)
        {
            throw new ProductDomainException("Quantity must be greater than zero");
        }
        
        if (!HasSufficientStock(quantity))
        {
            throw new ProductDomainException("Stock must be greater than zero");
        }

        Stock -= quantity;
    }

    public void AddStock(int quantity)
    {
        if (quantity <= 0)
        {
            throw new ProductDomainException("Quantity must be greater than zero");
        }
        
        Stock += quantity;
    }

    public void Reservate(int quantity, Guid shoppingCartId)
    {
        if (Stock - quantity - TotalReserved < 0)
        {
            throw new ProductDomainException("Not enough stock");
        }

        var _reservation = Reservations.FirstOrDefault(x => x.ShoppingCartId == shoppingCartId);
        if (_reservation != null)
        {
            _reservations.Remove(_reservation);
        }
        _reservations.Add(Reservation.Create(shoppingCartId, quantity));
    }
    
    public void CancelReservation(Guid shoppingCartId)
    {
        var _reservation = Reservations.FirstOrDefault(x => x.ShoppingCartId == shoppingCartId);
        if (_reservation != null)
        {
            _reservations.Remove(_reservation);
        }
    }

    private void DumpReservationUntilQuantityThreshold(int quantityThreshold)
    {
        var reservations = _reservations.OrderBy(x => x.CreatedAt)
            .TakeWhile(x => (quantityThreshold -= x.Quantity) >= 0)
            .ToList();
        _reservations.Except(reservations)
            .Select(r => new ReservationCanceledDueToStockUpdateEvent(r.ShoppingCartId, Id, r.Quantity))
            .ToList()
            .ForEach(r => AddDomainEvent(r));
        _reservations = reservations;
    }


    public void CommitReservation(Guid shoppingCartId)
    {
        var _reservation = Reservations.FirstOrDefault(x => x.ShoppingCartId == shoppingCartId);
        if (_reservation != null)
        {
            _reservations.Remove(_reservation);
            RemoveStock(_reservation.Quantity);
        }
    }
}