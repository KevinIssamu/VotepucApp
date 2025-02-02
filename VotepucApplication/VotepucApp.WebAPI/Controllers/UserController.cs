using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VotepucApp.Application.Cases.Shared;
using VotepucApp.Application.Cases.UseCases.CreateUser;
using VotepucApp.Application.Cases.UseCases.SelectUser.Requests;
using VotepucApp.Application.Cases.UseCases.SelectUser.Responses;
using VotepucApp.Application.Cases.UseCases.SelectUser.Validators;
using VotepucApp.Application.Cases.UseCases.Shared.Requests;
using VotepucApp.Application.Cases.UseCases.Shared.Responses;
using VotepucApp.Application.Cases.UseCases.Shared.Validators;
using VotepucApp.Application.Cases.UseCases.UpdateUser;
using VotepucApp.Application.ViewModels;
using VotepucApp.Persistence.Context.Seeder.Permissions;

namespace VotepucApp.WebAPI.Controllers;

public class UserController(IMediator mediator) : BaseController
{
    [Authorize(Policy = ElectionPermissions.ElectionGet)]
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<UserResponse>> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var request = new SelectUserByIdRequest<UserResponse>(id);
        return await HandleRequest<SelectUserByIdRequest<UserResponse>, UserResponse>(request, mediator,
            new SelectUserByIdValidator<UserResponse>(), cancellationToken);
    }
    
    [Authorize(Policy = ElectionPermissions.ElectionGet)]
    [HttpGet]
    public async Task<ActionResult<SelectedUsersResponse>> Get([FromQuery] SelectUsersRequest request,
        CancellationToken cancellationToken)
    {
        return await HandleRequest<SelectUsersRequest, SelectedUsersResponse>(request, mediator,
            new SelectUsersValidator(), cancellationToken);
    }
    
    [Authorize(Policy = UserPermissions.UserGet)]
    [HttpGet("{id:guid}/elections")]
    public async Task<ActionResult<SelectedUserElectionResponse>> GetUserElections(
        [FromRoute] Guid id, [FromQuery] int take, [FromQuery] int skip, CancellationToken cancellationToken)
    {
        var request = new SelectUserElectionsRequest(id, take, skip);
        return await HandleRequest<SelectUserElectionsRequest, SelectedUserElectionResponse>(request, mediator,
            new SelectElectionsFromUsersValidator(), cancellationToken);
    }

    [Authorize(Policy = UserPermissions.UserCreate)]
    [HttpPost]
    [AllowAnonymous]
    public async Task<ActionResult<GenericResponse>> Post(CreateUserRequest request,
        CancellationToken cancellationToken)
    {
        return await HandleRequest<CreateUserRequest, GenericResponse>(request, mediator, new CreateUserValidator(),
            cancellationToken);
    }
    
    [Authorize(Policy = UserPermissions.UserUpdate)]
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<UpdateUserResponse>> Put(
        [FromRoute] Guid id, [FromBody] UserRequestViewModel user, CancellationToken cancellationToken)
    {
        var request = new UpdateUserRequest(id, user);
        return await HandleRequest<UpdateUserRequest, UpdateUserResponse>(request, mediator, new UpdateUserValidator(),
            cancellationToken);
    }
}