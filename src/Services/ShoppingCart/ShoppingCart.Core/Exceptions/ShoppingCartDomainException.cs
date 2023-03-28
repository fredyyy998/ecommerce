namespace ShoppingCart.Core.Exceptions;

public class ShoppingCartDomainException : Exception
{
    public ShoppingCartDomainException(string message) : base(message)
    {
    }
}