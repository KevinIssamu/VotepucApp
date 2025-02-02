using FluentValidation;

namespace VotepucApp.Application.Cases.UseCases.UpdateUser;

public class UpdateUserValidator : AbstractValidator<UpdateUserRequest>
{
    public UpdateUserValidator()
    {
        RuleFor(x => x.UserRequestUpdated.UserName).NotEmpty();
    }
}