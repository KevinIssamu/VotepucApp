using Domain.ElectionAggregate.Participant.Enumerations;
using Domain.Shared;

namespace Domain.ElectionAggregate.Participant;

public abstract class Participant : BaseEntity
{
    public string Email { get; init; }
    public string Name { get; init; }
    public TypeOfParticipantEnum TypeOfParticipant { get; init; }
    public Guid ElectionId { get; init; }
    public Election.Election Election { get; init; }

    public Participant() { }
}