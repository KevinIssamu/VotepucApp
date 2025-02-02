using System.Security.Claims;
using Domain.Shared.AppError;
using Domain.Shared.AppSuccess;
using Domain.UserAggregate.Permissions;
using Domain.UserAggregate.User;
using OneOf;

namespace VotepucApp.Persistence.Interfaces;

public interface IClaimsService
{
    Task<OneOf<User, AppError>> GetUserByClaims(ClaimsPrincipal userClaims);
    Task<OneOf<AppSuccess, AppError>> AddUserToRoleAsync(User user, string roleName);
    Task<OneOf<AppSuccess, AppError>> CreateRoleAsync(string roleName);
    Task<OneOf<AppSuccess, AppError>> DeleteRoleAsync(string roleName);
    Task<OneOf<AppSuccess, AppError>> AddPermissionsToRoleAsync(string roleName, List<Permission> permissions);
}