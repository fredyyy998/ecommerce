namespace Fulfillment.Core.Buyer;

public class PersonalInformation
{
    public PersonalInformation(string firstName, string lastName, string email)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
    }

    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }
}