using Domain.ElectionAggregate.Election;
using Domain.ElectionAggregate.Election.Enumerations;
using Domain.ElectionAggregate.Participant;
using Domain.ElectionAggregate.VoteLink;
using Domain.Shared.Interfaces;
using MediatR;
using Microsoft.Extensions.Configuration;
using VotepucApp.Application.BusinessService.ElectionService;
using VotepucApp.Application.BusinessService.VoteLinkService;
using VotepucApp.Application.Cases.Shared;
using VotepucApp.Services.Interfaces;
using VotepucApp.Services.Interfaces.ConfigInterfaces;

namespace VotepucApp.Application.Cases.ElectionCases.UpdateElection.UpdateVote;

public class UpdateVoteHandler(
    IElectionService electionService,
    VoteLinkService voteLinkService,
    ITokenService tokenService,
    IJwtSettings jwtSettings,
    IUnitOfWork unitOfWork,
    IConfiguration configuration) : BaseHandler(configuration), IRequestHandler<UpdateVoteRequest, GenericResponse>
{
    private readonly IConfiguration _configuration = configuration;

    public async Task<GenericResponse> Handle(UpdateVoteRequest request, CancellationToken cancellationToken)
    {
        var election = await electionService.SelectByIdAsNoTrackingAsync(request.ElectionId, cancellationToken);

        if (election.IsT1)
            return CreateAppErrorResponse(election.AsT1);

        if (election.AsT0.Progress != ElectionProgressEnum.Active)
            return new GenericResponse(400, "Election is not active");

        var candidates =
            await electionService.SelectElectionCandidatesAsNoTrackingAsync(request.ElectionId, cancellationToken);

        if (candidates.IsT1)
            return CreateAppErrorResponse(candidates.AsT1);

        var voteLink = await voteLinkService.GetVoteLinkTracking(request.LinkId, cancellationToken);

        if (voteLink.IsT1)
            return CreateAppErrorResponse(voteLink.AsT1);

        var validateTokenResult = tokenService.ValidateRefreshToken(voteLink.AsT0.Token, jwtSettings);

        if (validateTokenResult.IsT1)
            return CreateAppErrorResponse(validateTokenResult.AsT1);

        if (!election.AsT0.MultiVote && request.CandidatesIds.Count > 1)
            return new GenericResponse(400, "The election does not allow more than one vote.");

        var validationResult = ValidateCandidates(request.CandidatesIds, candidates.AsT0);

        if (!validationResult.IsValid)
            return new GenericResponse(400, $"Invalid candidate IDs: {string.Join(", ", validationResult.InvalidIds)}",
            [
                new Link("get-candidates", $"election/{request.ElectionId}/candidates/{request.LinkId}", "GET",
                    _configuration)
            ]);

        MarkCandidatesAsVoted(validationResult.ValidCandidates);

        var invalidateLink = voteLink.AsT0.RemoveToken();

        if (invalidateLink.IsT1)
            return CreateAppErrorResponse(invalidateLink.AsT1);

        await unitOfWork.CommitAsync(cancellationToken);

        return CreateSuccessResponse("Votes successfully registered.");
    }

    private (bool IsValid, List<Guid> InvalidIds, List<Candidate> ValidCandidates) ValidateCandidates(
        List<Guid> candidateIds,
        List<Candidate> candidates)
    {
        var candidateDictionary = candidates.ToDictionary(c => c.Id);
        var validCandidates = new List<Candidate>();
        var invalidIds = new List<Guid>();

        foreach (var candidateId in candidateIds)
        {
            if (candidateDictionary.TryGetValue(candidateId, out var candidate))
                validCandidates.Add(candidate);
            else
                invalidIds.Add(candidateId);
        }

        return (invalidIds.Count == 0, invalidIds, validCandidates);
    }

    private static void MarkCandidatesAsVoted(List<Candidate> candidates)
    {
        foreach (var candidate in candidates)
        {
            candidate.SetVoted();
        }
    }
}