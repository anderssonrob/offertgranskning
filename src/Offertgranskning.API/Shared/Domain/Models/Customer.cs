using FluentValidation;

namespace Offertgranskning.API.Shared.Domain.Models;

public sealed record Customer(string FirstName, string LastName, string Email);

public sealed class CustomerValidator : AbstractValidator<Customer>
{
    public CustomerValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .MaximumLength(50);
            
        RuleFor(x => x.LastName)
            .NotEmpty()
            .MaximumLength(100);
            
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(320); // RFC 5321 limit
    }
}
