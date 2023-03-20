namespace Inventory.Core;

public class ProductDomainException : Exception
{
    public ProductDomainException(string message) : base(message)
    {
    }
}