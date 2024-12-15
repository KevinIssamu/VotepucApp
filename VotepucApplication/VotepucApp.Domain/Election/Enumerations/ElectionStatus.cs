using Domain.Shared;

namespace Domain.Election.Enumerations;

public class ElectionStatus(int id, string name) : Enumeration(id, name)
{
    public static readonly ElectionStatus Pending = new(1, nameof(Pending));
    public static readonly ElectionStatus Approved = new(2, nameof(Approved));
    public static readonly ElectionStatus Rejected = new(3, nameof(Rejected));
}