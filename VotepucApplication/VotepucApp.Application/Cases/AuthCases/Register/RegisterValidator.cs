using Domain.Shared.AppError.Constants;
using Domain.Shared.Constants;
using FluentValidation;

namespace VotepucApp.Application.Cases.AuthCases.Register;

public class RegisterValidator : AbstractValidator<RegisterRequest>
{
    public RegisterValidator()
    {
        RuleFor(x => x.Name).MaximumLength(LengthProperties.PersonNameMaxLength).NotEmpty();
        RuleFor(x => x.Email).EmailAddress();
        RuleFor(x => x.Password).NotEmpty();
        RuleFor(x => x.ConfirmPassword).NotEmpty();
    }
}