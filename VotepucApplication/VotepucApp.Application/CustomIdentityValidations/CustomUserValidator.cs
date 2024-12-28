using Microsoft.AspNetCore.Identity;

namespace VotepucApp.Application.CustomIdentityValidations;

public class CustomUserValidator<TUser>(IdentityErrorDescriber errors) : UserValidator<TUser>(errors)
    where TUser : class
{
    public override async Task<IdentityResult> ValidateAsync(UserManager<TUser> manager, TUser user)
    {
        var result = await base.ValidateAsync(manager, user);
        
        var errors = result.Errors;
        var filteredErrors = errors.Where(e => e.Code != "DuplicateUserName").ToList();

        return filteredErrors.Count == 0
            ? IdentityResult.Success
            : IdentityResult.Failed(filteredErrors.ToArray());
    }
}