using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using VotepucApp.Services.Authorization.Requirements;

namespace VotepucApp.Services.Authorization.Handlers;

public class OwnerResourceHandler : AuthorizationHandler<OwnerResourceRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OwnerResourceRequirement requirement)
    {
        if (!context.User.Identity.IsAuthenticated)
            return Task.CompletedTask;
        
        var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId is null)
            return Task.CompletedTask;
        
        if (context.Resource is string resourceId && resourceId == userId)
            context.Succeed(requirement);

        return Task.CompletedTask;
    }
}