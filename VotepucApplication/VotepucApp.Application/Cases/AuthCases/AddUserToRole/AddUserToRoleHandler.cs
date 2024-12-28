using Domain.UserAggregate.User;
using MediatR;
using Microsoft.AspNetCore.Identity;
using VotepucApp.Application.Cases.Shared;

namespace VotepucApp.Application.Cases.AuthCases.AddUserToRole;

public class AddUserToRoleHandler(UserManager<User> userManager)
    : IRequestHandler<AddUserToRoleRequest, GenericResponse>
{
    public async Task<GenericResponse> Handle(AddUserToRoleRequest request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.Email);

        if (user is null)
            return new GenericResponse(400, "Unable to find user.");

        var result = await userManager.AddToRoleAsync(user, request.RoleName);

        return result.Succeeded
            ? new GenericResponse(200, $"User {user.Email} added to the {request.RoleName} role.")
            : new GenericResponse(500, $"Error: Unable to add user {user.Email} to the {request.RoleName} role.");
    }
}