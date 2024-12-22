using MediatR;
using Microsoft.AspNetCore.Mvc;
using VotepucApp.Application.Cases.UseCases.CreateUser;
using VotepucApp.Application.Cases.UseCases.SelectUser.Requests;
using VotepucApp.Application.Cases.UseCases.SelectUser.Validators;
using VotepucApp.Application.Cases.UseCases.Shared.Requests;
using VotepucApp.Application.Cases.UseCases.Shared.Responses;
using VotepucApp.Application.Cases.UseCases.Shared.Validators;
using VotepucApp.Application.Cases.UseCases.UpdateUser;
using VotepucApp.Application.ViewModels;

namespace VotepucApp.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController(IMediator mediator) : ControllerBase
{
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<UserResponse>> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var request = new SelectUserByIdRequest<UserResponse>(id);
        var validatior = new SelectUserByIdValidator<UserResponse>();
        var validationResult = await validatior.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var response = await mediator.Send(request, cancellationToken);

        if (response.AppError != null)
            return BadRequest(response.AppError.Message);

        return Ok(response.User);
    }

    [HttpGet]
    public async Task<ActionResult<List<UserResponse>>> GetAll([FromQuery] SelectUsersRequest request,
        CancellationToken cancellationToken)
    {
        var validatior = new SelectUsersValidator();
        var validationResult = await validatior.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var response = await mediator.Send(request, cancellationToken);

        if (response.ErrorMessage != null)
            return BadRequest(response.ErrorMessage.Message);

        return Ok(response.Users);
    }

    [HttpGet("{id:guid}/elections")]
    public async Task<ActionResult<List<UserResponse>>> GetUserElections([FromRoute] Guid id, [FromQuery] int take,
        [FromQuery] int skip,
        CancellationToken cancellationToken)
    {
        var request = new SelectUserElectionsRequest(id, take, skip);

        var validation = new SelectElectionsFromUsersValidator();
        var validationResult = await validation.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var response = await mediator.Send(request, cancellationToken);

        if (response.ErrorMessage != null)
            return BadRequest(response.ErrorMessage);

        return Ok(response.Elections);
    }

    [HttpPost]
    public async Task<ActionResult<UserResponse>> Post(CreateUserRequest request,
        CancellationToken cancellationToken)
    {
        var validator = new CreateUserValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var response = await mediator.Send(request, cancellationToken);

        if (response.AppError != null)
            return BadRequest(response.AppError.Message);

        return CreatedAtAction(
            nameof(GetById),
            new { id = response?.User?.Id },
            response
        );
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<UserResponse>> Put([FromRoute] Guid id, [FromBody] UserRequestViewModel user, CancellationToken cancellationToken)
    {
        var request = new UpdateUserRequest(id, user);
        
        var validator = new UpdateUserValidator();
        
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return BadRequest(validationResult.Errors);

        var response = await mediator.Send(request, cancellationToken);

        if (response.Error != null)
            return BadRequest(response.Error.Message);

        return Ok(response);
    }
}