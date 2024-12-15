using Domain.Participant.Enumerations;
using Domain.Shared;

namespace Domain.Participant;

public abstract class Participant : BaseEntity
{
    public string Email { get; init; }
    public string Name { get; init; }
    public TypeOfParticipant TypeOfParticipant { get; init; }
    public Guid ElectionId { get; init; }
    public Election.Election Election { get; init; }
}