using Domain.ElectionAggregate.Election.Enumerations;
using Domain.Shared;
using Domain.UserAggregate.User;

namespace Domain.ElectionAggregate.Election;

public abstract class Election : BaseEntity, IAggregateRoot
{
    public string Title { get; protected set; }
    public string Description { get; protected set; }
    public string EmailInvitationText { get; protected set; }
    public bool MultiVote { get; init; }
    public DateTimeOffset StartDate { get; protected set; }
    public DateTimeOffset EndDate { get; protected set; }
    public ElectionStatusEnum Status { get; protected set; }
    public ElectionProgressEnum Progress { get; protected set; }
    public Guid OwnerId { get; init; }
    public User Owner { get; init; }
    public ICollection<Participant.Participant> Participants { get; protected set; }
    public ICollection<VoteLink.VoteLink> VoteLinks { get; protected set; }

    public Election() { }
}