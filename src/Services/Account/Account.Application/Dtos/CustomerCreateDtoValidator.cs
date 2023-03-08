using FluentValidation;

namespace Account.Application.Dtos;

public class CustomerCreateDtoValidator : AbstractValidator<CustomerCreateDto>
{
    public CustomerCreateDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();
        
        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(6);
    }
}