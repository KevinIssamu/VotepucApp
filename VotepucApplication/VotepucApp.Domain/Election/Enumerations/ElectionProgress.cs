using Domain.Shared;

namespace Domain.Election.Enumerations;

public class ElectionProgress(int id, string name) : Enumeration(id, name)
{
    public static readonly ElectionProgress Inactive = new(1, nameof(Inactive));
    public static readonly ElectionProgress Active = new(2, nameof(Active));
    public static readonly ElectionProgress Finished = new(3, nameof(Finished));
}