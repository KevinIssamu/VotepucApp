using Domain.Shared.AppError;
using FluentValidation;
using VotepucApp.Application.Cases.UseCases.SelectUser.Requests;

namespace VotepucApp.Application.Cases.UseCases.SelectUser.Validators;

public class SelectUsersValidator : AbstractValidator<SelectUsersRequest>
{
    public SelectUsersValidator()
    {
        RuleFor(x => x.Top)
            .GreaterThan(0)
            .WithMessage(new AppError("'Top' must be greater than zero.", AppErrorTypeEnum.ValidationFailure).Message);
    
        RuleFor(x => x.Skip)
            .GreaterThanOrEqualTo(0)
            .WithMessage(new AppError("'Skip' must be zero or greater.", AppErrorTypeEnum.ValidationFailure).Message);
    }
}