using Domain.ElectionAggregate.Election.Enumerations;
using Domain.Shared.AppError;
using Domain.Shared.Interfaces;
using MediatR;
using Microsoft.Extensions.Configuration;
using VotepucApp.Application.BusinessService.ElectionService;
using VotepucApp.Application.BusinessService.VoteLinkService;
using VotepucApp.Application.Cases.ElectionCases.SelectElection.Requests;
using VotepucApp.Application.Cases.ElectionCases.SelectElection.Responses;
using VotepucApp.Application.Cases.Shared;
using VotepucApp.Application.ViewModels;
using VotepucApp.Services.Interfaces;
using VotepucApp.Services.Interfaces.ConfigInterfaces;

namespace VotepucApp.Application.Cases.ElectionCases.SelectElection.Handlers;

public class SelectElectionCandidates(
    IElectionService electionService,
    VoteLinkService voteLinkService,
    ITokenService tokenService,
    IJwtSettings jwtSettings,
    IConfiguration configuration)
    : BaseHandler(configuration), IRequestHandler<SelectElectionCandidatesRequest, GenericResponse>
{
    public async Task<GenericResponse> Handle(SelectElectionCandidatesRequest request,
        CancellationToken cancellationToken)
    {
        var link = await voteLinkService.GetVoteLinkTracking(request.LinkId, cancellationToken);
        if (link.IsT1)
            return CreateAppErrorResponse(link.AsT1);

        var validationRefreshToken = tokenService.ValidateRefreshToken(link.AsT0.Token, jwtSettings);
        if (validationRefreshToken.IsT1)
            return CreateAppErrorResponse(validationRefreshToken.AsT1);

        var election = await electionService.SelectByIdAsNoTrackingAsync(request.ElectionId, cancellationToken);
        if (election.IsT1)
            return CreateAppErrorResponse(election.AsT1);

        if (election.AsT0.Progress != ElectionProgressEnum.Active)
            return CreateAppErrorResponse(new AppError("Election progress not active",
                AppErrorTypeEnum.BusinessRuleValidationFailure));

        if (link.AsT0.ElectionId != election.AsT0.Id)
            return new GenericResponse(400, "Invalid link");

        var candidates =
            await electionService.SelectElectionCandidatesAsNoTrackingAsync(election.AsT0.Id, cancellationToken);
        if (candidates.IsT1)
            return CreateAppErrorResponse(candidates.AsT1);

        var candidatesViewModel = candidates.AsT0.Select(c => new CandidateViewModel(c.Id, c.Name, c.Email)).ToList();

        return new SelectElectionCandidatesResponse(200, $"Total candidates: {candidatesViewModel.Count}",
            candidatesViewModel);
    }
}