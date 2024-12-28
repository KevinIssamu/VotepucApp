using Domain.Shared.AppError.Constants;
using Domain.Shared.Constants;
using FluentValidation;
using VotepucApp.Application.Cases.UseCases.SelectUser.Requests;

namespace VotepucApp.Application.Cases.UseCases.SelectUser.Validators;

public sealed class SelectUserByEmailValidator : AbstractValidator<SelectUserByEmailRequest>
{
    public SelectUserByEmailValidator()
    {
        RuleFor(e => e.Email).EmailAddress();
    }
}