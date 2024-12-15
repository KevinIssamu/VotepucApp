using Domain.Shared;

namespace Domain.Participant.Enumerations;

public class TypeOfParticipant(int id, string name) : Enumeration(id, name)
{
    public static readonly TypeOfParticipant Candidate = new(1, nameof(Candidate));
    public static readonly TypeOfParticipant Voter = new(2, nameof(Voter));
}