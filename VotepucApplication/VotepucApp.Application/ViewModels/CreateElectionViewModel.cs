namespace VotepucApp.Application.ViewModels;

//Request recebida pela controller
public sealed record CreateElectionViewModel(
    string Title,
    string Description,
    string InvitationMessage,
    bool AllowMultipleVotes,
    DateTimeOffset StartDate,
    DateTimeOffset EndDate,
    ICollection<ParticipantViewModel> Participants);