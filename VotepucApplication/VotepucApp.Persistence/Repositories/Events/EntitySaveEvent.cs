using VotepucApp.Persistence.Repositories.Events.Enums;

namespace VotepucApp.Persistence.Repositories.Events;

public record EntitySaveEvent(string EntityName, string Message, SaveStatus Status);