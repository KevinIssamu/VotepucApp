using Domain.Shared.Constants;
using FluentValidation;

namespace VotepucApp.Application.Cases.AuthCases.CreateRole;

public class CreateRoleValidator : AbstractValidator<CreateRoleRequest>
{
    public CreateRoleValidator()
    {
        RuleFor(x => x.RoleName)
            .MaximumLength(LengthProperties.RoleNameMaxLength)
            .NotEmpty();
    }
}