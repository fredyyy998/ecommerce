using Account.Core.Common;

namespace Account.Core.User;

public class PersonalInformation : ValueObject
{
    public string FirstName { get; protected set; }
    public string LastName { get; protected set; }
    public DateOnly DateOfBirth { get; protected set; }
    
    protected PersonalInformation() { }
    
    public PersonalInformation(string firstName, string lastName, DateOnly dateOfBirth)
    {
        FirstName = firstName;
        LastName = lastName;
        DateOfBirth = dateOfBirth;
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return FirstName;
        yield return LastName;
        yield return DateOfBirth;
    }
}