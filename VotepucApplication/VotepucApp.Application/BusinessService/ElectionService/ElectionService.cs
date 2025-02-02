using Domain.ElectionAggregate.Election;
using Domain.ElectionAggregate.Election.Enumerations;
using Domain.ElectionAggregate.Participant;
using Domain.ElectionAggregate.Participant.Enumerations;
using Domain.ElectionAggregate.VoteLink;
using Domain.Shared.AppError;
using Domain.Shared.AppSuccess;
using Domain.Shared.Interfaces;
using Domain.UserAggregate.User;
using OneOf;
using VotepucApp.Application.ViewModels;
using VotepucApp.Services.Interfaces;

namespace VotepucApp.Application.BusinessService.ElectionService;

public class ElectionService(IElectionRepository electionRepository) : IElectionService
{
    public async Task<OneOf<Election, AppError>> SelectByIdTrackingAsync(Guid id, CancellationToken cancellationToken)
    {
        var electionExistResult = await electionRepository.SelectByIdTrackingAsync(id, cancellationToken);

        if (electionExistResult.IsT1)
            return electionExistResult.AsT1;

        if (electionExistResult.AsT0 == null)
            return new AppError($"Election with ID '{id}' was not found.", AppErrorTypeEnum.NotFound);

        return electionExistResult.AsT0;
    }

    public async Task<OneOf<Election, AppError>> SelectByIdAsNoTrackingAsync(Guid id,
        CancellationToken cancellationToken)
    {
        var electionExistResult = await electionRepository.SelectByIdAsNoTrackingAsync(id, cancellationToken);

        if (electionExistResult.IsT1)
            return electionExistResult.AsT1;

        if (electionExistResult.AsT0 == null)
            return new AppError($"Election with ID '{id}' was not found.", AppErrorTypeEnum.NotFound);

        return electionExistResult.AsT0;
    }

    public async Task<OneOf<List<Election>, AppError>> SelectPaginatedAsync(int skip, int top,
        CancellationToken cancellationToken)
    {
        var electionResult = await electionRepository.SelectPaginatedAsync(top, skip, cancellationToken);

        if (electionResult.IsT1)
            return electionResult.AsT1;

        if (electionResult.AsT0.Count == 0)
            return new AppError("No elections found for the given parameters.", AppErrorTypeEnum.NotFound);

        return electionResult.AsT0;
    }

    public async Task<OneOf<List<Candidate>, AppError>> SelectElectionCandidatesAsNoTrackingAsync(Guid electionId,
        CancellationToken cancellationToken)
    {
        var candidates = await electionRepository.SelectCandidates(electionId, cancellationToken);

        if (candidates.IsT1)
            return candidates.AsT1;

        if (candidates.AsT0 is null || candidates.AsT0.Count == 0)
            return new AppError("No Candidates found.", AppErrorTypeEnum.NotFound);

        return candidates.AsT0;
    }

    public async Task<OneOf<List<VoteLink>, AppError>> SelectElectionVoteLinksAsync(Guid electionId,
        CancellationToken cancellationToken)
    {
        var voteLinks = await electionRepository.SelectVoteLinks(electionId, cancellationToken);

        if (voteLinks.IsT1)
            return voteLinks.AsT1;

        if (voteLinks.AsT0 is null || voteLinks.AsT0.Count == 0)
            return new AppError("No Vote Links found.", AppErrorTypeEnum.NotFound);

        return voteLinks.AsT0;
    }

    public OneOf<AppSuccess, AppError> Delete(Election election, CancellationToken cancellationToken)
    {
        var deleteResult = electionRepository.Delete(election, cancellationToken);

        if (deleteResult.IsT1)
            return deleteResult.AsT1;

        return new AppSuccess("Election successfully deleted.");
    }

    public OneOf<AppSuccess, AppError> Approve(Election election)
    {
        var pendingElection = election.GetBehavior<PendingElectionBehavior>();

        if (pendingElection.IsT1)
            return new AppError("Only pending elections can have their status changed.",
                AppErrorTypeEnum.BusinessRuleValidationFailure);

        var approveElectionResult = pendingElection.AsT0.Approve();

        if (approveElectionResult.IsT1)
            return approveElectionResult.AsT1;

        return new AppSuccess("Election approved successfully.");
    }

    public OneOf<AppSuccess, AppError> Reject(Election election)
    {
        var pendingElection = election.GetBehavior<PendingElectionBehavior>();

        if (pendingElection.IsT1)
            return new AppError("Only pending elections can have their status changed.",
                AppErrorTypeEnum.BusinessRuleValidationFailure);

        var approveElectionResult = pendingElection.AsT0.Reject();

        if (approveElectionResult.IsT1)
            return approveElectionResult.AsT1;

        return new AppSuccess("Election rejected.");
    }

    public OneOf<AppSuccess, AppError> Start(Election election)
    {
        var approvedElection = election.GetBehavior<ApprovedElectionBehavior>();
        if (approvedElection.IsT1)
            return new AppError("Only approved elections can have their progress changed.",
                AppErrorTypeEnum.BusinessRuleValidationFailure);

        if (election.Progress == ElectionProgressEnum.Active)
            return new AppError("Election already is activated.", AppErrorTypeEnum.BusinessRuleValidationFailure);

        var startElection = approvedElection.AsT0.Start();
        if (startElection.IsT1)
            return startElection.AsT1;

        return startElection.AsT0;
    }

    public OneOf<AppSuccess, AppError> Starting(Election election)
    {
        var approvedElection = election.GetBehavior<ApprovedElectionBehavior>();
        if (approvedElection.IsT1)
            return new AppError("Only approved elections can have their progress changed.",
                AppErrorTypeEnum.BusinessRuleValidationFailure);
        
        if (election.Progress != ElectionProgressEnum.Inactive)
            return new AppError("Election must be in inactive progress to be in starting status.",
                AppErrorTypeEnum.BusinessRuleValidationFailure);
        
        var startingElection = approvedElection.AsT0.Starting();
        if (startingElection.IsT1)
            return startingElection.AsT1;

        return startingElection.AsT0;
    }

    public OneOf<AppSuccess, AppError> Finish(Election election)
    {
        var approvedElection = election.GetBehavior<ApprovedElectionBehavior>();

        if (approvedElection.IsT1)
            return new AppError("Only approved elections can have their progress changed.",
                AppErrorTypeEnum.BusinessRuleValidationFailure);

        if (election.Progress != ElectionProgressEnum.Active)
            return new AppError("Only started elections can have their progress changed.",
                AppErrorTypeEnum.BusinessRuleValidationFailure);

        var finishElection = approvedElection.AsT0.Finish();

        if (finishElection.IsT1)
            return finishElection.AsT1;

        return finishElection.AsT0;
    }

    public async Task<OneOf<AppSuccess, AppError>> CreateAsync(
        string title,
        string description,
        string invitationMessage,
        bool allowMultipleVotes,
        DateTimeOffset startDate,
        DateTimeOffset endDate,
        User owner,
        ICollection<ParticipantViewModel> participantsReq,
        CancellationToken cancellationToken)
    {
        var election = new Election(title, description,
            invitationMessage, allowMultipleVotes, startDate,
            endDate, owner, null);

        var (candidateSuccesses, candidateErrors) = participantsReq
            .Where(p => p.TypeOfParticipant == TypeOfParticipantEnum.Candidate)
            .Select(c => Candidate.Factory.Create(c.Email, c.Name, election))
            .Aggregate(
                (Valid: new List<Candidate>(), Errors: new List<AppError>()),
                (acc, next) =>
                {
                    if (next.IsT0) acc.Valid.Add(next.AsT0);
                    if (next.IsT1) acc.Errors.Add(next.AsT1);
                    return acc;
                });

        var (voterSuccesses, voterErrors) = participantsReq
            .Where(p => p.TypeOfParticipant == TypeOfParticipantEnum.Voter)
            .Select(p => Voter.Factory.Create(p.Email, p.Name, election))
            .Aggregate(
                (Valid: new List<Voter>(), Errors: new List<AppError>()),
                (acc, next) =>
                {
                    if (next.IsT0) acc.Valid.Add(next.AsT0);
                    if (next.IsT1) acc.Errors.Add(next.AsT1);
                    return acc;
                });

        if (candidateErrors.Count > 0 || voterErrors.Count > 0)
        {
            var allErrors = new List<AppError>();

            allErrors.AddRange(candidateErrors);
            allErrors.AddRange(voterErrors);

            return new AppError(allErrors.ToString()!, AppErrorTypeEnum.BusinessRuleValidationFailure);
        }

        var participants = new List<Participant>();
        participants.AddRange(candidateSuccesses);
        participants.AddRange(voterSuccesses);

        var pendingElection = election.GetBehavior<PendingElectionBehavior>();

        pendingElection.AsT0.SetParticipants(participants);

        var createElectionResult = await electionRepository.CreateAsync(election, cancellationToken);

        if (createElectionResult.IsT1)
            return createElectionResult.AsT1;

        return createElectionResult.AsT0;
    }

    public OneOf<AppSuccess, AppError> Update(Election election, CancellationToken cancellationToken)
    {
        var updateElectionResult = electionRepository.Update(election, cancellationToken);

        if (updateElectionResult.IsT1)
            return updateElectionResult.AsT1;

        return updateElectionResult.AsT0;
    }
}