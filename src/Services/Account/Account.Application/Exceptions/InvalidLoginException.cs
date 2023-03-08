namespace Account.Application.Exceptions;

public class InvalidLoginException : Exception
{
    public InvalidLoginException(string message) : base(message)
    {
        
    }
}