using MassTransit;
using VotepucApp.Services.RabbitMQ.Interfaces;

namespace VotepucApp.Services.RabbitMQ;

public class MassTransitEventPublisher(IBus bus) : IEventPublisher
{
    public async Task Publish<TEvent>(TEvent eventMessage) where TEvent : class
    {
        await bus.Publish(eventMessage);
    }
}