namespace Fulfillment.Infrastructure;

public class UnknownEventKeyException : Exception
{
    public UnknownEventKeyException(string key) : base($"Unknown event key: {key}")
    {
    }
}