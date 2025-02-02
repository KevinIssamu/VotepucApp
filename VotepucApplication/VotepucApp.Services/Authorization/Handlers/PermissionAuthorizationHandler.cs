using System.Security.Claims;
using Domain.UserAggregate.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using VotepucApp.Services.Authorization.Requirements;

namespace VotepucApp.Services.Authorization.Handlers;

public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public PermissionAuthorizationHandler(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        if (context.User.Identity is not { IsAuthenticated: true })
        {
            context.Fail();
            return;
        }
        
        var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            context.Fail();
            return;
        }
        
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            context.Fail();
            return;
        }

        var roles = await _userManager.GetRolesAsync(user);
        
        foreach (var roleName in roles)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null) continue;

            var claims = await _roleManager.GetClaimsAsync(role);
            if (!claims.Any(c => c.Type == "Permission" && c.Value == requirement.Permission)) continue;
            
            context.Succeed(requirement);
            return;
        }

        context.Fail();
    }
}