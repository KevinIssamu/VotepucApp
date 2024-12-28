using Domain.ElectionAggregate.Participant.Enumerations;

namespace VotepucApp.Application.ViewModels;

public record ParticipantViewModel(string Name, string Email, TypeOfParticipantEnum  TypeOfParticipant);