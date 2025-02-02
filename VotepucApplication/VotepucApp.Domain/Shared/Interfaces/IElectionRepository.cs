using Domain.ElectionAggregate.Election;
using Domain.ElectionAggregate.Participant;
using Domain.ElectionAggregate.VoteLink;
using OneOf;

namespace Domain.Shared.Interfaces;

public interface IElectionRepository : IRepository<Election>
{
    Task<OneOf<List<Participant>, AppError.AppError>> SelectElectionParticipants(Guid electionId, CancellationToken cancellationToken);
    Task<OneOf<int, AppError.AppError>> CountElectionParticipants(Guid electionId, CancellationToken cancellationToken);
    Task<OneOf<List<Voter>, AppError.AppError>> SelectVoters(Guid electionId, CancellationToken cancellationToken);
    Task<OneOf<List<Candidate>, AppError.AppError>> SelectCandidates(Guid electionId, CancellationToken cancellationToken);
    Task<OneOf<List<VoteLink>, AppError.AppError>> SelectVoteLinks(Guid electionId, CancellationToken cancellationToken);
}