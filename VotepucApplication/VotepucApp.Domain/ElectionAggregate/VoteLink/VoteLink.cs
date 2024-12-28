using Domain.Shared;

namespace Domain.ElectionAggregate.VoteLink;

public class VoteLink : BaseEntity
{
    public string Token { get; private set; }
    public Guid ElectionId { get; init; }
    public Election.Election Election { get; protected set; }

    public void SetToken(string token)
    {
        Token = token;
    }

    public VoteLink(string token, Election.Election election)
    {
        Id = Guid.NewGuid();
        Token = token;
        ElectionId = election.Id;
        Election = election;
        CreateAt = DateTimeOffset.Now;
    }
}