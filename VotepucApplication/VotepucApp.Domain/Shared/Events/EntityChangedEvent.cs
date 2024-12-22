using Domain.Shared.Events.EventsStatus;
using MediatR;

namespace Domain.Shared.Events;

public class EntityChangedEvent(string propName, string message, PropChangedEventStatusEnum status)
    : INotification
{
    public string PropName { get; } = propName;
    public string Messsage { get; } = message;
    public PropChangedEventStatusEnum Status { get; } = status;
}