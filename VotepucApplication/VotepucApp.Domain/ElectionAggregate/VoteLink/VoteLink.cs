using Domain.ElectionAggregate.VoteLink.Enumerations;
using Domain.Shared;
using Domain.Shared.AppSuccess;

namespace Domain.ElectionAggregate.VoteLink;

public abstract class VoteLink : BaseEntity
{
    public string Token { get; init; }
    public bool WasUtilized { get; protected set; }
    public DateTimeOffset ExpirationDate { get; protected set; }
    public VoteLinkStatusEnum Status { get; protected set; }
    public Guid ElectionId { get; init; }
    public Election.Election Election { get; protected set; }

    public AppSuccess SetExpirationDate()
    {
        ExpirationDate = Election.EndDate;
        UpdatedAt = DateTimeOffset.Now;
        return new AppSuccess("Expiration date successfully modified.");
    }

    public VoteLink()
    {
    }
}