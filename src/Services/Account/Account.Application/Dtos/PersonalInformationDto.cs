using System.ComponentModel.DataAnnotations;

namespace Account.Application.Dtos;

public class PersonalInformationDto
{
    [Required]
    public string FirstName { get; set; }
    
    [Required]
    public string LastName { get; set; }
    
    [Required]
    [DataType(DataType.Date)]
    public string DateOfBirth { get; set; }
    
    public PersonalInformationDto(
        string firstName,
        string lastName,
        string dateOfBirth)
    {
        FirstName = firstName;
        LastName = lastName;
        DateOfBirth = dateOfBirth;
    }
}
