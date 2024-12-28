using Domain.Shared.Constants;
using FluentValidation;

namespace VotepucApp.Application.Cases.AuthCases.AddUserToRole;

public class AddUserToRoleValidator : AbstractValidator<AddUserToRoleRequest>
{
    public AddUserToRoleValidator()
    {
        RuleFor(x => x.Email).EmailAddress();
        RuleFor(x => x.RoleName).MaximumLength(LengthProperties.RoleNameMaxLength).NotEmpty();
    }
}