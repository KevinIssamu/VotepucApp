using Domain.ElectionAggregate.Election;
using Domain.ElectionAggregate.Participant;
using Domain.ElectionAggregate.VoteLink;
using Domain.Shared.AppError;
using Domain.Shared.AppSuccess;
using Domain.UserAggregate.User;
using OneOf;
using VotepucApp.Application.ViewModels;
using VotepucApp.Services.Interfaces;

namespace VotepucApp.Application.BusinessService.ElectionService;

public interface IElectionService : IService<Election>
{
    Task<OneOf<AppSuccess, AppError>> CreateAsync(
        string title,
        string description,
        string invitationMessage,
        bool allowMultipleVotes, 
        DateTimeOffset startDate,
        DateTimeOffset endDate,
        User owner,
        ICollection<ParticipantViewModel> participants, 
        CancellationToken cancellationToken);
    OneOf<AppSuccess, AppError> Update(Election election, CancellationToken cancellationToken);
    OneOf<AppSuccess, AppError> Approve(Election election);
    OneOf<AppSuccess, AppError> Reject(Election election);
    OneOf<AppSuccess, AppError> Start(Election election);
    OneOf<AppSuccess, AppError> Starting(Election election);
    OneOf<AppSuccess, AppError> Finish(Election election);
    Task<OneOf<List<Candidate>, AppError>> SelectElectionCandidatesAsNoTrackingAsync(Guid electionId,
        CancellationToken cancellationToken);
    Task<OneOf<List<VoteLink>, AppError>> SelectElectionVoteLinksAsync(Guid electionId,
        CancellationToken cancellationToken);
}