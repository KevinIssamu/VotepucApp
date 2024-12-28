using Domain.Shared.AppError.Constants;
using Domain.Shared.Constants;
using FluentValidation;

namespace VotepucApp.Application.Cases.UseCases.CreateUser;

public sealed class CreateUserValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserValidator()
    {
        RuleFor(x => x.Email).NotEmpty().MaximumLength(LengthProperties.PersonEmailMaxLength).EmailAddress();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(LengthProperties.PersonNameMaxLength);
        RuleFor(x => x.Password).NotEmpty();
    }
}