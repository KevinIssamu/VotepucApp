using Domain.ElectionAggregate.Election;
using Domain.UserAggregate.User;
using VotepucApp.Persistence.Repositories.Events.Enums;

namespace VotepucApp.Persistence.Repositories.Events.UserEvents;

public record SearchUserElectionsEvent(Guid UserId, List<Election>? Elections, string Message, SearchStatus Status);