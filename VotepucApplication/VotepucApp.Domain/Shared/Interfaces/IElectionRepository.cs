using Domain.ElectionAggregate.Election;
using Domain.ElectionAggregate.Participant;
using OneOf;

namespace Domain.Shared.Interfaces;

public interface IElectionRepository : IRepository<Election>
{
    Task<OneOf<List<Participant>, AppError.AppError>> SelectElectionParticipants(Guid electionId, CancellationToken cancellationToken);
    
    Task<OneOf<int, AppError.AppError>> CountElectionParticipants(Guid electionId, CancellationToken cancellationToken);
}