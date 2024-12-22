using MediatR;
using VotepucApp.Persistence.Repositories.Events.Enums;

namespace VotepucApp.Persistence.Repositories.Events;

public record EntityUpdateEvent(string EntityName, string Message, UpdateStatus Status) : INotification;