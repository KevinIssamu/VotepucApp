using Domain.Shared.Events;
using Domain.Shared.Events.EventsStatus;
using MediatR;

namespace Domain.Shared;

public abstract class BaseEntity
{
    public Guid Id { get; init; }
    public DateTimeOffset CreateAt { get; init; }
    public DateTimeOffset? UpdatedAt { get; set; }
}