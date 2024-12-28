using MediatR;
using Microsoft.AspNetCore.Identity;
using VotepucApp.Application.Cases.Shared;

namespace VotepucApp.Application.Cases.AuthCases.CreateRole;

public class CreateRoleHandler(RoleManager<IdentityRole> roleManager)
    : IRequestHandler<CreateRoleRequest, GenericResponse>
{
    public async Task<GenericResponse> Handle(CreateRoleRequest request, CancellationToken cancellationToken)
    {
        var roleExist = await roleManager.RoleExistsAsync(request.RoleName);
        if (roleExist)
            return new GenericResponse(400, "Role already exists.");

        var roleResult = await roleManager.CreateAsync(new IdentityRole(request.RoleName));

        return roleResult.Succeeded
            ? new GenericResponse(200, $"Role {request.RoleName} added successfully.")
            : new GenericResponse(400, $"Issue adding the new {request.RoleName} role.");
    }
}