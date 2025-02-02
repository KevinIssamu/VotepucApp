using Domain.UserAggregate.User;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VotepucApp.Application.Cases.AuthCases;
using VotepucApp.Application.Cases.AuthCases.AddUserToRole;
using VotepucApp.Application.Cases.AuthCases.CreateRole;
using VotepucApp.Application.Cases.AuthCases.DeleteRole;
using VotepucApp.Application.Cases.AuthCases.Login;
using VotepucApp.Application.Cases.AuthCases.RefreshToken;
using VotepucApp.Application.Cases.AuthCases.Register;
using VotepucApp.Application.Cases.Shared;
using VotepucApp.Persistence.Context.Seeder.Permissions;

namespace VotepucApp.WebAPI.Controllers;

public class AuthController(UserManager<User> userManager, IMediator mediator) : BaseController
{
    [Authorize(Policy = AuthPermissions.AuthCreateRole)]
    [HttpPost("CreateRole")]
    public async Task<ActionResult<GenericResponse>> CreateRole([FromBody] CreateRoleRequest request)
    {
        return await HandleRequest<CreateRoleRequest, GenericResponse>(request, mediator, new RoleNameValidator());
    }

    [Authorize(Policy = AuthPermissions.AuthDeleteRole)]
    [HttpDelete("DeleteRole")]
    public async Task<ActionResult<GenericResponse>> DeleteRole([FromBody] DeleteRoleRequest request)
    {
        return await HandleRequest<DeleteRoleRequest, GenericResponse>(request, mediator);
    }
    
    [Authorize(Policy = AuthPermissions.AuthAddUserToRole)]
    [HttpPost("AddUserToRole")]
    public async Task<ActionResult<GenericResponse>> AddUserToRole([FromBody] AddUserToRoleRequest request)
    {
        return await HandleRequest<AddUserToRoleRequest, GenericResponse>(request, mediator, new AddUserToRoleValidator());
    }
    
    [AllowAnonymous]
    [HttpPost("Login")]
    public async Task<ActionResult<GenericResponse>> Login([FromBody] LoginRequest loginRequest)
    {
        return await HandleRequest<LoginRequest, GenericResponse>(loginRequest, mediator, new LoginValidator());
    }
    
    [AllowAnonymous]
    [HttpPost("Register")]
    public async Task<ActionResult<GenericResponse>> Register([FromBody] RegisterRequest request)
    {
        return await HandleRequest<RegisterRequest, GenericResponse>(request, mediator, new RegisterValidator());
    }
    
    [HttpPost("RefreshToken")]
    public async Task<ActionResult<GenericResponse>> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        return await HandleRequest<RefreshTokenRequest, GenericResponse>(request, mediator, new RefreshTokenValidator());
    }
    
    [Authorize(Policy = AuthPermissions.AuthRevoke)]
    [HttpPost("Revoke/{id:guid}")]
    public async Task<ActionResult<GenericResponse>> Revoke(Guid id)
    {
        var user = await userManager.FindByIdAsync(id.ToString());
        if (user is null)
            return BadRequest("User not found or invalid.");

        user.RefreshToken = null;
        await userManager.UpdateAsync(user);

        return NoContent();
    }
}
