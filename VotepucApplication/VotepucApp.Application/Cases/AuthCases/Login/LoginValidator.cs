using Domain.Shared.Constants;
using FluentValidation;

namespace VotepucApp.Application.Cases.AuthCases.Login;

public class LoginValidator : AbstractValidator<LoginRequest>
{
    public LoginValidator()
    {
        RuleFor(x => x.Email).EmailAddress();
        RuleFor(x => x.Password).NotEmpty();
    }
}