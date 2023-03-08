namespace Account.Application.Dtos;

public record CustomerCreateDto(
    string Email,
    string Password
);