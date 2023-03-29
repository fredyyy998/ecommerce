namespace ShoppingCart.Core.Exceptions;

public class ProductDomainException : Exception
{
    public ProductDomainException(string message) : base(message)
    {
    }
}