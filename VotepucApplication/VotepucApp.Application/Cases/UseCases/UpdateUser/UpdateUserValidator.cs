using FluentValidation;

namespace VotepucApp.Application.Cases.UseCases.UpdateUser;

public class UpdateUserValidator : AbstractValidator<UpdateUserRequest>
{
    public UpdateUserValidator()
    {
        RuleFor(x => x.UserRequestUpdated.Email).EmailAddress().NotEmpty();
        RuleFor(x => x.UserRequestUpdated.UserName).NotEmpty();
        RuleFor(x => x.UserRequestUpdated.UserType).NotEmpty();
    }
}