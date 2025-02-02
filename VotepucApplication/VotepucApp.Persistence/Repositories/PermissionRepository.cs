using Domain.Shared.AppError;
using Domain.Shared.AppSuccess;
using Domain.Shared.Interfaces;
using Domain.UserAggregate.Permissions;
using Microsoft.EntityFrameworkCore;
using OneOf;
using VotepucApp.Persistence.Context;

namespace VotepucApp.Persistence.Repositories;

public class PermissionRepository(AppDbContext appDbContext, ReadDbContext readDbContext) : IPermissionRepository
{
    public OneOf<AppSuccess, AppError> AddPermission(Permission permission)
    {
        try
        {
            appDbContext.Permissions.Add(permission);
            
            return new AppSuccess("Permission added successfully");
        }
        catch (Exception e)
        {
            return new AppError(e.Message, AppErrorTypeEnum.SystemError);
        }
    }

    public async Task<OneOf<AppSuccess, AppError>> AddRangePermissionAsync(List<Permission> permissions)
    {
        try
        {
            await appDbContext.Permissions.AddRangeAsync(permissions);
            
            return new AppSuccess("Permissions added successfully");
        }
        catch (Exception e)
        {
            return new AppError(e.Message, AppErrorTypeEnum.SystemError);
        }
    }

    public OneOf<Permission, AppError> GetPermission(string permissionName)
    {
        try
        {
            return readDbContext.Permissions.FirstOrDefault(p => p.Name == permissionName);
        }
        catch (Exception e)
        {
            return new AppError(e.Message, AppErrorTypeEnum.SystemError);
        }
    }

    public async Task<OneOf<List<Permission>, AppError>> GetAllPermissionsAsync()
    {
        try
        {
            return await readDbContext.Permissions.ToListAsync();
        }
        catch (Exception e)
        {
            return new AppError(e.Message, AppErrorTypeEnum.SystemError);
        }
    }
    
    public async Task<bool> AnyAsync()
    {
        return await readDbContext.Permissions.AnyAsync();
    }
}