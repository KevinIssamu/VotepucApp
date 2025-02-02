
namespace VotepucApp.Services.RabbitMQ.Interfaces;

public interface IEventPublisher
{
    Task Publish<TEvent>(TEvent eventMessage) where TEvent : class;
}