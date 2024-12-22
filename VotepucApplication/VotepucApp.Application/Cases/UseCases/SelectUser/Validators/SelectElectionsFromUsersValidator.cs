using Domain.Shared.AppError;
using Domain.UserAggregate.User;
using FluentValidation;
using VotepucApp.Application.Cases.UseCases.SelectUser.Requests;

namespace VotepucApp.Application.Cases.UseCases.SelectUser.Validators;

public class SelectElectionsFromUsersValidator : AbstractValidator<SelectUserElectionsRequest>
{
    public SelectElectionsFromUsersValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage(new AppError($"{nameof(User)} cannot be null or empty.", AppErrorTypeEnum.ValidationFailure).Message);
    
        RuleFor(x => x.Skip)
            .GreaterThanOrEqualTo(0)
            .WithMessage(new AppError("'Skip' must be greater than zero.", AppErrorTypeEnum.ValidationFailure).Message);
    
        RuleFor(x => x.Take)
            .GreaterThan(0)
            .WithMessage(new AppError("'Take' must be greater than zero.", AppErrorTypeEnum.ValidationFailure).Message);
    }
}