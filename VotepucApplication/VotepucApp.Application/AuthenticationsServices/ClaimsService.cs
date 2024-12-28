using System.Security.Claims;
using Domain.Shared.AppError;
using Domain.UserAggregate.User;
using Microsoft.AspNetCore.Identity;
using OneOf;

namespace VotepucApp.Application.AuthenticationsServices;

public class ClaimsService(UserManager<User> userManager)
{
    public async Task<OneOf<User, AppError>> GetUserByClaims(ClaimsPrincipal userClaims)
    {
        var user = await userManager.GetUserAsync(userClaims);

        if (user is null)
            return new AppError("No user found", AppErrorTypeEnum.NotFound);

        return user;
    }
}