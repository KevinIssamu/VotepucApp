using FluentValidation;

namespace VotepucApp.Application.Cases.AuthCases.RefreshToken;

public class RefreshTokenValidator : AbstractValidator<RefreshTokenRequest>
{
    public RefreshTokenValidator()
    {
        RuleFor(x => x.RefreshToken).NotEmpty();
        RuleFor(x => x.AccessToken).NotEmpty();
    }
}