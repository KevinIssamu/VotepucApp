using Domain.ElectionAggregate.Election;
using Domain.ElectionAggregate.Election.Enumerations;
using Domain.Shared.AppError;
using Domain.Shared.AppSuccess;
using Domain.Shared.Interfaces;
using OneOf;
using VotepucApp.Application.Cases.ElectionCases.CreateElection;
using VotepucApp.Application.Cases.ElectionCases.UpdateElection;
using VotepucApp.Application.Cases.Shared;
using VotepucApp.Application.Cases.UseCases.CreateUser;
using VotepucApp.Application.Cases.UseCases.SelectUser.Requests;
using VotepucApp.Application.Cases.UseCases.Shared.Requests;
using VotepucApp.Application.Cases.UseCases.Shared.Responses;
using VotepucApp.Application.Cases.UseCases.UpdateUser;

namespace VotepucApp.Application.BusinessService.ElectionService;

public class ElectionService(IElectionRepository electionRepository, IUnitOfWork unitOfWork) : IElectionService
{
    public async Task<OneOf<Election, AppError>> SelectByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var electionExistResult = await electionRepository.SelectByIdAsync(id, cancellationToken);
        
        if (electionExistResult.IsT1)
            return electionExistResult.AsT1;

        if (electionExistResult.AsT0 == null)
            return new AppError($"Election with ID '{id}' was not found.", AppErrorTypeEnum.NotFound);

        return electionExistResult.AsT0;
    }
    public async Task<OneOf<List<Election>, AppError>> SelectPaginatedAsync(int skip, int top, CancellationToken cancellationToken)
    {
        var electionResult = await electionRepository.SelectPaginatedAsync(top, skip, cancellationToken);

        if (electionResult.IsT1)
            return electionResult.AsT1;

        if (electionResult.AsT0.Count == 0)
            return new AppError("No elections found for the given parameters.", AppErrorTypeEnum.NotFound);

        return electionResult.AsT0;
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
        if (election.Status != ElectionStatusEnum.Pending)
            return new AppError("Only pending elections can have their status changed.", AppErrorTypeEnum.BusinessRuleValidationFailure);
        
        var pendingElection = (PendingElection)election;
        var approveElectionResult = pendingElection.Approve();

        if (approveElectionResult.IsT1)
            return approveElectionResult.AsT1;

        return new AppSuccess("Election approved successfully.");
    }
    
    public OneOf<AppSuccess, AppError> Reject(Election election)
    {
        if (election.Status != ElectionStatusEnum.Pending)
            return new AppError("Only pending elections can have their status changed.", AppErrorTypeEnum.BusinessRuleValidationFailure);
        
        var pendingElection = (PendingElection)election;
        var approveElectionResult = pendingElection.Reject();

        if (approveElectionResult.IsT1)
            return approveElectionResult.AsT1;

        return new AppSuccess("Election rejected.");
    }
    
    public OneOf<AppSuccess, AppError> Start(Election election)
    {
        if (election.Status != ElectionStatusEnum.Approved)
            return new AppError("Only approved elections can have their progress changed.", AppErrorTypeEnum.BusinessRuleValidationFailure);
        
        if(election.Progress == ElectionProgressEnum.Active)
            return new AppError("Election already is actived.", AppErrorTypeEnum.BusinessRuleValidationFailure);

        var approvedElection = (ElectionApproved)election;

        var startElection = approvedElection.Start();

        if (startElection.IsT1)
            return startElection.AsT1;

        return new AppSuccess("Election started successfully.");
    }
    
    public OneOf<AppSuccess, AppError> Finish(Election election)
    {
        if (election.Status != ElectionStatusEnum.Approved)
            return new AppError("Only approved elections can have their progress changed.", AppErrorTypeEnum.BusinessRuleValidationFailure);
        
        if(election.Progress != ElectionProgressEnum.Active)
            return new AppError("Only strated elections can have their progress changed.", AppErrorTypeEnum.BusinessRuleValidationFailure);

        var approvedElection = (ElectionApproved)election;

        var finishElection = approvedElection.Finish();

        if (finishElection.IsT1)
            return finishElection.AsT1;

        return new AppSuccess("Election finished.");
    }

    public async Task<OneOf<Election, AppError>> CreateAsync(Election election, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<OneOf<AppSuccess, AppError>> UpdateAsync(Election request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}