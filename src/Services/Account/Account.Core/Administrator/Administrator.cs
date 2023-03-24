using Account.Core.User;

namespace Account.Core.Administrator;

public class Administrator : User.User
{
    public string Role { get; protected set; }
    
    public static Administrator Create(string email, string password)
    {
        return new Administrator()
        {
            Id = Guid.NewGuid(),
            Email = email,
            Password = Password.Create(password),
            Role = "Admin",
            CreatedAt = DateTime.Now,
        };
    }
}