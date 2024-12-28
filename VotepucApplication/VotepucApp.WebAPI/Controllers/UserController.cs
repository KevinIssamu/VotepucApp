using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VotepucApp.Application.Cases.UseCases.CreateUser;
using VotepucApp.Application.Cases.UseCases.SelectUser.Requests;
using VotepucApp.Application.Cases.UseCases.SelectUser.Responses;
using VotepucApp.Application.Cases.UseCases.SelectUser.Validators;
using VotepucApp.Application.Cases.UseCases.Shared.Requests;
using VotepucApp.Application.Cases.UseCases.Shared.Responses;
using VotepucApp.Application.Cases.UseCases.Shared.Validators;
using VotepucApp.Application.Cases.UseCases.UpdateUser;
using VotepucApp.Application.ViewModels;

namespace VotepucApp.WebAPI.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class UserController(IMediator mediator) : ControllerBase
{
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<UserResponse>> GetById(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var request = new SelectUserByIdRequest<UserResponse>(id);
        var validatior = new SelectUserByIdValidator<UserResponse>();
        var validationResult = await validatior.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var response = await mediator.Send(request, cancellationToken);

        return response;
    }

    [HttpGet]
    public async Task<ActionResult<SelectedUsersResponse>> GetAll(
        [FromQuery] SelectUsersRequest request,
        CancellationToken cancellationToken)
    {
        var validatior = new SelectUsersValidator();
        var validationResult = await validatior.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var response = await mediator.Send(request, cancellationToken);

        return response;
    }

    [HttpGet("{id:guid}/elections")]
    public async Task<ActionResult<SelectedUserElectionResponse>> GetUserElections(
        [FromRoute] Guid id, 
        [FromQuery] int take,
        [FromQuery] int skip,
        CancellationToken cancellationToken)
    {
        var request = new SelectUserElectionsRequest(id, take, skip);

        var validation = new SelectElectionsFromUsersValidator();
        var validationResult = await validation.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var response = await mediator.Send(request, cancellationToken);

        return response;
    }

    [Authorize(Policy = "SuperAdm")]
    [HttpPost]
    [AllowAnonymous]
    public async Task<ActionResult<UserResponse>> Post(
        CreateUserRequest request,
        CancellationToken cancellationToken)
    {
        var validator = new CreateUserValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var response = await mediator.Send(request, cancellationToken);

        return response;
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<UpdateUserResponse>> Put(
        [FromRoute] Guid id, 
        [FromBody] UserRequestViewModel user,
        CancellationToken cancellationToken)
    {
        var request = new UpdateUserRequest(id, user);

        var validator = new UpdateUserValidator();

        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var response = await mediator.Send(request, cancellationToken);

        return response;
    }
}