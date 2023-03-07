using Account.Core.Common;

namespace Account.Core.User;

public abstract class User : Entity
{
    public string Email { get; protected set; }
    public Password Password { get; protected set; }
    public DateTime CreatedAt { get; protected set; }
    public DateTime UpdatedAt { get; protected set; }
}