using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Account.Infrastructure.MessageBus;

public interface IMessageBus
{
    Object Publish<T>(string topic, T message);
}