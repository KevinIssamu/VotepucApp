using Domain.ElectionAggregate.Election.Enumerations;
using Domain.Shared;
using Domain.Shared.AppError;
using Domain.Shared.Interfaces;
using Domain.UserAggregate.User;
using OneOf;

namespace Domain.ElectionAggregate.Election;

public class Election : BaseEntity, IAggregateRoot
{
    public string Title { get; internal set; }
    public string Description { get; internal set; }
    public string EmailInvitationText { get; internal set; }
    public bool MultiVote { get; init; }
    public DateTimeOffset StartDate { get; internal set; }
    public DateTimeOffset EndDate { get; internal set; }
    public ElectionStatusEnum ElectionStatus { get; internal set; }
    public ElectionProgressEnum Progress { get; internal set; }
    public string OwnerId { get; init; }
    public User Owner { get; init; }
    public ICollection<Participant.Participant>? Participants { get; internal set; }
    public ICollection<VoteLink.VoteLink>? VoteLinks { get; internal set; }
    
    public OneOf<T, AppError> GetBehavior<T>() where T : class
    {
        return ElectionStatus switch
        {
            ElectionStatusEnum.Approved when typeof(T) == typeof(ApprovedElectionBehavior) => new ApprovedElectionBehavior(this) as T,
            ElectionStatusEnum.Rejected when typeof(T) == typeof(RejectedElectionBehavior) => new RejectedElectionBehavior(this) as T,
            ElectionStatusEnum.Pending when typeof(T) == typeof(PendingElectionBehavior) => new PendingElectionBehavior(this) as T,
            _ => new AppError("Invalid behavior type or status.", AppErrorTypeEnum.BusinessRuleValidationFailure)
        };
    }
    
    public Election() { }

    public Election(
        string title,
        string description,
        string invitationMessage,
        bool allowMultipleVotes,
        DateTimeOffset startDate,
        DateTimeOffset endDate,
        User owner,
        List<Participant.Participant>? participants)
    {
        Id = Guid.NewGuid();
        Title = title;
        Description = description;
        EmailInvitationText = invitationMessage;
        MultiVote = allowMultipleVotes;
        StartDate = startDate;
        EndDate = endDate;
        Owner = owner;
        OwnerId = owner.Id;
        Participants = participants;
        ElectionStatus = ElectionStatusEnum.Pending;
        Progress = ElectionProgressEnum.Inactive;
        CreateAt = DateTimeOffset.Now;
    }
}