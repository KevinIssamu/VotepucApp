using Domain.Election.Enumerations;
using Domain.Shared;
using OneOf;

namespace Domain.Election;

public abstract class Election : BaseEntity
{
    public string Title { get; protected set; }
    public string Description { get; protected set; }
    public string EmailInvitationText { get; protected set; }
    public bool MultiVote { get; init; }
    public DateTimeOffset StartDate { get; protected set; }
    public DateTimeOffset EndDate { get; protected set; }
    public ElectionStatus Status { get; protected set; }
    public ElectionProgress Progress { get; protected set; }
    public Guid OwnerId { get; init; }
    public User.User Owner { get; init; }
    public ICollection<Participant.Participant> Participants { get; protected set; }
    public ICollection<VoteLink.VoteLink> VoteLinks { get; protected set; }
}