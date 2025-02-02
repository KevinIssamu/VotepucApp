using System.Security.Claims;
using Domain.Shared.AppError;
using Domain.Shared.AppSuccess;
using Domain.UserAggregate.Permissions;
using Domain.UserAggregate.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using OneOf;
using VotepucApp.Persistence.Interfaces;

namespace VotepucApp.Services.AuthenticationsServices;

public class ClaimsService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, ILogger<ClaimsService> logger) : IClaimsService
{
    public async Task<OneOf<User, AppError>> GetUserByClaims(ClaimsPrincipal userClaims)
    {
        var user = await userManager.GetUserAsync(userClaims);

        if (user is null)
            return new AppError("No user found", AppErrorTypeEnum.NotFound);

        return user;
    }

    public async Task<OneOf<AppSuccess, AppError>> AddUserToRoleAsync(User user, string roleName)
    {
        if (string.IsNullOrWhiteSpace(roleName))
            return new AppError("Role name cannot be null or empty", AppErrorTypeEnum.BusinessRuleValidationFailure);

        try
        {
            var result = await userManager.AddToRoleAsync(user, roleName);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return new AppError($"Failed to add role '{roleName}' to user '{user.UserName}': {errors}",
                    AppErrorTypeEnum.SystemError);
            }

            return new AppSuccess($"Successfully added role '{roleName}' to user '{user.UserName}'");
        }
        catch (Exception ex)
        {
            return new AppError($"An unexpected error occurred: {ex.Message}", AppErrorTypeEnum.SystemError);
        }
    }

    public async Task<OneOf<AppSuccess, AppError>> CreateRoleAsync(string roleName)
    {
        if(await roleManager.RoleExistsAsync(roleName))
            return new AppError("Role already exists", AppErrorTypeEnum.ValidationFailure);
        
        if(string.IsNullOrWhiteSpace(roleName))
            return new AppError("Role name cannot be null or empty", AppErrorTypeEnum.BusinessRuleValidationFailure);

        try
        {
            var result = await roleManager.CreateAsync(new IdentityRole(roleName));
            if (result.Succeeded) 
                return new AppSuccess($"Successfully created role '{roleName}'");
            
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return new AppError($"Failed to create role '{roleName}': {errors}", AppErrorTypeEnum.SystemError);
        }
        catch (Exception e)
        {
            return new AppError($"An unexpected error occurred: {e.Message}", AppErrorTypeEnum.SystemError);
        }
    }

    public async Task<OneOf<AppSuccess, AppError>> DeleteRoleAsync(string roleName)
    {
        if(!await roleManager.RoleExistsAsync(roleName))
            return new AppError("Role not found.", AppErrorTypeEnum.NotFound);

        try
        {
            var role = await roleManager.FindByNameAsync(roleName);
            if (role == null)
                return new AppError("Role not found.", AppErrorTypeEnum.NotFound);
            
            var result = await roleManager.DeleteAsync(role);
            if(result.Succeeded)
                return new AppSuccess($"Successfully deleted role '{roleName}'");
            
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return new AppError($"Failed to delete role '{roleName}': {errors}", AppErrorTypeEnum.SystemError);
        }
        catch (Exception e)
        {
            return new AppError($"An unexpected error occurred: {e.Message}", AppErrorTypeEnum.SystemError);
        }
    }

    public async Task<OneOf<AppSuccess, AppError>> AddPermissionsToRoleAsync(string roleName, List<Permission> permissions)
    {
        var role = await roleManager.FindByNameAsync(roleName);
        if(role is null)
            return new AppError("Role not found.", AppErrorTypeEnum.NotFound);
        
        var claims = await roleManager.GetClaimsAsync(role);

        foreach (var permission in permissions)
        {
            if (claims.Any(c => c.Type == "Permission" && c.Value == permission.Name))
            {
                logger.LogInformation($"Permission {permission} is already exists for role {roleName}");
                continue;
            }
            
            var result = await roleManager.AddClaimAsync(role, new Claim("Permission", permission.Name));
            if(!result.Succeeded)
                return new AppError($"Failed to add permission to role {roleName}.", AppErrorTypeEnum.SystemError);
        }
        return new AppSuccess($"Successfully added permission to role {roleName}");
    }
}