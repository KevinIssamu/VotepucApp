using Microsoft.AspNetCore.Authorization;

namespace VotepucApp.Services.Authorization.Requirements;

public class OwnerResourceRequirement : IAuthorizationRequirement
{
    public OwnerResourceRequirement() { }
}