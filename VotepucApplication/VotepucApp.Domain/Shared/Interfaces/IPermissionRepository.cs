using Domain.UserAggregate.Permissions;
using OneOf;

namespace Domain.Shared.Interfaces;

public interface IPermissionRepository
{
    OneOf<AppSuccess.AppSuccess, AppError.AppError> AddPermission(Permission permission);
    Task<OneOf<AppSuccess.AppSuccess, AppError.AppError>> AddRangePermissionAsync(List<Permission> permissions);
    OneOf<Permission, AppError.AppError> GetPermission(string permissionName);
    Task<OneOf<List<Permission>, AppError.AppError>> GetAllPermissionsAsync();
}