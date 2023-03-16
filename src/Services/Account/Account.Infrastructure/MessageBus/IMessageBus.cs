using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Account.Infrastructure.MessageBus;

public interface IMessageBus
{
    void Publish(string topic, string message);
}