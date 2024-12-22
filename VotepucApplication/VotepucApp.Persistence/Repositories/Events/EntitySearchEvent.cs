using Domain.Shared;
using MediatR;
using VotepucApp.Persistence.Repositories.Events.Enums;

namespace VotepucApp.Persistence.Repositories.Events;

public record EntitySearchEvent<T>(List<T>? Entities, string Message, SearchStatus Status) : INotification;