namespace Inventory.Application.Exceptions;

public class NoActiveShoppingBasketFoundException : EntityNotFoundException
{
    public NoActiveShoppingBasketFoundException(string message) : base(message)
    {
    }
}