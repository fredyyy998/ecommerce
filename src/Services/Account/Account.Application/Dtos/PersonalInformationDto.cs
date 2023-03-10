namespace Account.Application.Dtos;

public record PersonalInformationDto(
    string FirstName,
    string LastName,
    string DateOfBirth);