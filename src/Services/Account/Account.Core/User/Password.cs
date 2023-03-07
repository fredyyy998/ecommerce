using Account.Core.Common;

namespace Account.Core.User;

public class Password : ValueObject
{
    public byte [] Hash { get; private set; }
    public byte [] Salt { get; private set; }
    
    public Password() { }
    
    public Password(byte [] hash, byte [] salt)
    {
        Hash = hash;
        Salt = salt;
    }
    
    public static Password Create(string password)
    {
        using (var hmac = new System.Security.Cryptography.HMACSHA512())
        {
            return new Password(hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password)), hmac.Key);
        }
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Hash;
        yield return Salt;
    }

    public bool Verify(string password)
    {
        using (var hmac = new System.Security.Cryptography.HMACSHA512(Salt))
        {
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != Hash[i]) return false;
            }
        }
        return true;
    }
}