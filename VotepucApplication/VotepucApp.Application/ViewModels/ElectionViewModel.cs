using Domain.ElectionAggregate.Election.Enumerations;

namespace VotepucApp.Application.ViewModels;

public record ElectionViewModel(
    Guid Id, 
    Guid OwnerId,
    string Title, 
    string Description, 
    string EmailInvitationText, 
    bool MultiVote, 
    ElectionStatusEnum Status,
    ElectionProgressEnum Progress,
    DateTimeOffset StartDate, 
    DateTimeOffset EndDate
    );