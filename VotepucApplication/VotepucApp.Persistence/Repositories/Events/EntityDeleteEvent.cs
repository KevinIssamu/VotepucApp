using MediatR;
using VotepucApp.Persistence.Repositories.Events.Enums;

namespace VotepucApp.Persistence.Repositories.Events;

public record EntityDeleteEvent(string EntityName, string Message, DeleteStatus Status) : INotification;