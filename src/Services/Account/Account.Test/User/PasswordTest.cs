using Account.Core.User;

namespace Account.Test.User;

public class PasswordTest
{
    [Fact]
    public void PasswordIsHashed()
    {
        var password = Password.Create("abc123");

        Assert.NotNull(password.Hash);
        Assert.NotNull(password.Salt);
    }

    [Fact]
    public void PasswordCanBeVerified()
    {
        var password = Password.Create("abc123");
        
        Assert.True(password.Verify("abc123"));
    }
}