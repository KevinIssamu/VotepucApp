using Domain.ElectionAggregate.Election.Enumerations;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VotepucApp.Application.Cases.ElectionCases.CreateElection;
using VotepucApp.Application.Cases.ElectionCases.SelectElection.Requests;
using VotepucApp.Application.Cases.ElectionCases.SelectElection.Responses;
using VotepucApp.Application.Cases.ElectionCases.Shared;
using VotepucApp.Application.Cases.ElectionCases.UpdateElection.UpdateElectionStatus;
using VotepucApp.Application.Cases.Shared;

namespace VotepucApp.WebAPI.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class ElectionController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<GenericResponse>> Post([FromBody] CreateElectionRequest request)
    {
        var validator = new CreateElectionValidator();
        var validationResult = await validator.ValidateAsync(request);

        if (!validationResult.IsValid)
        {
            var genericResponse = new GenericResponse(400, validationResult.Errors.ToString());
            return StatusCode(400, genericResponse.Message);
        }
        
        var authRequest = new AuthenticatedRequest<CreateElectionRequest, GenericResponse>(User, request);
        
        var response = await mediator.Send(authRequest);
        
        return StatusCode(response.StatusCode, response.Message);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<SelectElectionByIdResponse>> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var request = new ElectionIdRequest<SelectElectionByIdResponse>(id);

        var response = await mediator.Send(request, cancellationToken);

        return response;
    }
    
    [HttpGet]
    public async Task<ActionResult<SelectedElectionsResponse>> GetAll([FromQuery] SelectElectionsRequest request, CancellationToken cancellationToken)
    {
        var response = await mediator.Send(request, cancellationToken);

        return response;
    }

    [HttpPatch("/{id:guid}/status")]
    public async Task<ActionResult<GenericResponse>> ChangeStatus([FromRoute] Guid id, [FromBody] ElectionStatusEnum status)
    {
        var validator = new ElectionIdValidator();
        var request = new UpdateStatusRequest(id, status);
        
        var validatorResult = await validator.ValidateAsync(request);

        if (!validatorResult.IsValid)
            return new GenericResponse(400, validatorResult.Errors[0].ErrorMessage);

        var response = await mediator.Send(request);

        return response;
    }
    
    [HttpPatch("/{id:guid}/progress")]
    public async Task<ActionResult<GenericResponse>> ChangeProgress([FromRoute] Guid id, [FromBody] ElectionProgressEnum status)
    {
        
    }
}