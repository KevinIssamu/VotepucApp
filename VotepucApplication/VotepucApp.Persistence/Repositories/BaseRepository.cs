using MediatR;
using VotepucApp.Persistence.Repositories.Events;
using VotepucApp.Persistence.Repositories.Events.Enums;

namespace VotepucApp.Persistence.Repositories;

public class BaseRepository(IMediator mediator)
{
    protected async Task CreateSearchEvent<T>(List<T>? entities, string message, SearchStatus status)
    {
        var entitySearchEvent = new EntitySearchEvent<T>(entities, message, status);
        
        await mediator.Publish(entitySearchEvent);
    }
    protected async Task CreateSaveEvent(string entityName, string message, SaveStatus status)
    {
        var entitySaveEvent = new EntitySaveEvent(entityName, message, status);
        
        await mediator.Publish(entitySaveEvent);
    }
    protected async Task CreateUpdateEvent(string entityName, string message, UpdateStatus status)
    {
        var entityUpdateEvent = new EntityUpdateEvent(entityName, message, status);
        
        await mediator.Publish(entityUpdateEvent);
    }
    protected async Task CreateDeleteEvent(string entityName, string message, DeleteStatus status)
    {
        var entityDeleteEvent = new EntityDeleteEvent(entityName, message, status);

        await mediator.Publish(entityDeleteEvent);
    }
}