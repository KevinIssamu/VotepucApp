using Domain.Shared.AppError.Constants;
using FluentValidation;

namespace VotepucApp.Application.Cases.UseCases.CreateUser;

public sealed class CreateUserValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserValidator()
    {
        RuleFor(x => x.Email).NotEmpty().MaximumLength(ConstantsMaxLength.PersonEmailMaxLength).EmailAddress();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(ConstantsMaxLength.PersonNameMaxLength);
        RuleFor(x => x.Password).NotEmpty();
    }
}