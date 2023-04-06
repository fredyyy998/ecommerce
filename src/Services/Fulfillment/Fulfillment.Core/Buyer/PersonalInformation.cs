namespace Fulfillment.Core.Buyer;

public class PersonalInformation
{
    public PersonalInformation(string firstName, string lastName, string email)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
    }

    private string FirstName { get; set; }
    private string LastName { get; set; }
    private string Email { get; set; }
}