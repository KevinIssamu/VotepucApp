using Domain.ElectionAggregate.Participant.Enumerations;

namespace VotepucApp.Application.ViewModels;

public record ParticipantViewModel(Guid ElectionId, string Name, string Email, TypeOfParticipantEnum TypeOfParticipant);