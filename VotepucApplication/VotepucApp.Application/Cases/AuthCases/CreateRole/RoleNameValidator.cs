using Domain.Shared.Constants;
using FluentValidation;
using VotepucApp.Application.Cases.AuthCases.DeleteRole;

namespace VotepucApp.Application.Cases.AuthCases.CreateRole;

public class RoleNameValidator : AbstractValidator<CreateRoleRequest>
{
    public RoleNameValidator()
    {
        RuleFor(x => x.RoleName)
            .MaximumLength(LengthProperties.RoleNameMaxLength)
            .NotEmpty();
    }
}