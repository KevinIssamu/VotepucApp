using VotepucApp.Application.ViewModels;

namespace VotepucApp.Application.Cases.ElectionCases.CreateElection;

//Request recebida pela controller
public sealed record CreateElectionRequest(
    string Title,
    string Description,
    string InvitationMessage,
    bool AllowMultipleVotes,
    DateTimeOffset StartDate,
    DateTimeOffset EndDate,
    ICollection<ParticipantViewModel> Participantes);