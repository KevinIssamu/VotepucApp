using Domain.ElectionAggregate.Election;
using Domain.Shared.AppError;
using Domain.Shared.AppSuccess;
using OneOf;
using VotepucApp.Application.Cases.ElectionCases.CreateElection;
using VotepucApp.Application.Cases.ElectionCases.UpdateElection;
using VotepucApp.Application.Cases.UseCases.CreateUser;
using VotepucApp.Application.Cases.UseCases.UpdateUser;

namespace VotepucApp.Application.BusinessService.ElectionService;

public interface IElectionService : IService<Election>
{
    Task<OneOf<Election, AppError>> CreateAsync(Election election, CancellationToken cancellationToken);
    Task<OneOf<AppSuccess, AppError>> UpdateAsync(Election election, CancellationToken cancellationToken);
    OneOf<AppSuccess, AppError> Approve(Election election);
    OneOf<AppSuccess, AppError> Reject(Election election);
    OneOf<AppSuccess, AppError> Start(Election election);
    OneOf<AppSuccess, AppError> Finish(Election election);
}