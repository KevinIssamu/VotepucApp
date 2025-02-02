using Domain.ElectionAggregate.Election.Enumerations;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VotepucApp.Application.Cases.ElectionCases.CreateElection;
using VotepucApp.Application.Cases.ElectionCases.SelectElection.Requests;
using VotepucApp.Application.Cases.ElectionCases.SelectElection.Responses;
using VotepucApp.Application.Cases.ElectionCases.Shared;
using VotepucApp.Application.Cases.ElectionCases.UpdateElection.UpdateElectionProgress;
using VotepucApp.Application.Cases.ElectionCases.UpdateElection.UpdateElectionStatus;
using VotepucApp.Application.Cases.ElectionCases.UpdateElection.UpdateVote;
using VotepucApp.Application.Cases.Shared;
using VotepucApp.Application.ViewModels;
using VotepucApp.Persistence.Context.Seeder.Permissions;

namespace VotepucApp.WebAPI.Controllers;

public class ElectionController(IMediator mediator) : BaseController
{
    [Authorize(Policy = ElectionPermissions.ElectionCreate)]
    [HttpPost]
    public async Task<ActionResult<GenericResponse>> Post([FromBody] CreateElectionViewModel viewModel)
    {
        var authRequest = new AuthenticatedRequest<CreateElectionViewModel, GenericResponse>(User, viewModel);

        return await HandleRequest<AuthenticatedRequest<CreateElectionViewModel, GenericResponse>, GenericResponse>(
            authRequest, mediator, new CreateElectionValidator());
    }
    
    [Authorize(Policy = ElectionPermissions.ElectionGet)]
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<SelectElectionByIdResponse>> GetById([FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        var request = new ElectionIdRequest<SelectElectionByIdResponse>(id);
        return await HandleRequest<ElectionIdRequest<SelectElectionByIdResponse>, SelectElectionByIdResponse>(request,
            mediator, cancellationToken: cancellationToken);
    }
    
    [Authorize(Policy = ElectionPermissions.ElectionGet)]
    [HttpGet]
    public async Task<ActionResult<SelectedElectionsResponse>> GetAll([FromQuery] SelectElectionsRequest request,
        CancellationToken cancellationToken)
    {
        return await HandleRequest<SelectElectionsRequest, SelectedElectionsResponse>(request, mediator,
            cancellationToken: cancellationToken);
    }
    
    [Authorize(Policy = ElectionPermissions.ElectionDelete)]
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<GenericResponse>> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var request = new ElectionIdRequest<GenericResponse>(id);
        return await HandleRequest<ElectionIdRequest<GenericResponse>, GenericResponse>(request, mediator,
            cancellationToken: cancellationToken);
    }
    
    [Authorize(Policy = ElectionPermissions.ElectionChangeStatus)]
    [HttpPatch("{id:guid}/status")]
    public async Task<ActionResult<GenericResponse>> ChangeStatus([FromRoute] Guid id,
        [FromBody] ElectionStatusEnum status)
    {
        var request = new UpdateStatusRequest(id, status);
        return await HandleRequest<UpdateStatusRequest, GenericResponse>(request, mediator, new ElectionIdValidator());
    }
    
    [Authorize(Policy = ElectionPermissions.ElectionChangeProgress)]
    [HttpPatch("{id:guid}/progress")]
    public async Task<ActionResult<GenericResponse>> ChangeProgress([FromRoute] Guid id,
        [FromBody] ElectionProgressEnum status)
    {
        var request = new UpdateProgressRequest(id, status);
        return await HandleRequest<UpdateProgressRequest, GenericResponse>(request, mediator,
            new UpdateProgressValidator());
    }

    [AllowAnonymous]
    [HttpGet("{electionId:guid}/Candidates/{linkId:guid}")]
    public async Task<ActionResult<GenericResponse>> GetCandidates([FromRoute] Guid electionId, [FromRoute] Guid linkId)
    {
        var request = new SelectElectionCandidatesRequest(electionId, linkId);
        return await HandleRequest<SelectElectionCandidatesRequest, GenericResponse>(request, mediator);
    }

    [AllowAnonymous]
    [HttpPatch("{electionId:guid}/Vote/{linkId:guid}")]
    public async Task<ActionResult<GenericResponse>> Vote([FromRoute] Guid electionId, [FromRoute] Guid linkId,
        [FromBody] List<Guid> candidatesId)
    {
        var request = new UpdateVoteRequest(electionId, linkId, candidatesId);
        return await HandleRequest<UpdateVoteRequest, GenericResponse>(request, mediator);
    }
}