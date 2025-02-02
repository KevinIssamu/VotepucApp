using Domain.ElectionAggregate.VoteLink;
using OneOf;

namespace Domain.Shared.Interfaces;

public interface IVoteLinkRepository
{
    public Task<OneOf<AppSuccess.AppSuccess, AppError.AppError>> CreateAsync(List<VoteLink> voteLinks,
        CancellationToken cancellationToken);

    public Task<OneOf<VoteLink, AppError.AppError>> GetVoteLinkByIdAsNoTrackingAsync(Guid id, CancellationToken cancellationToken);
    public Task<OneOf<VoteLink, AppError.AppError>> GetVoteLinkByIdTrackingAsync(Guid id, CancellationToken cancellationToken);
}