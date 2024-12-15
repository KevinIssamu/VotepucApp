using Domain.Shared;

namespace Domain.VoteLink.Enumerations;

public class VoteLinkStatus(int id, string name) : Enumeration(id, name)
{
    public static readonly VoteLinkStatus Inactive = new VoteLinkStatus(1, nameof(Inactive));
    public static readonly VoteLinkStatus Active = new VoteLinkStatus(2, nameof(Active));
}