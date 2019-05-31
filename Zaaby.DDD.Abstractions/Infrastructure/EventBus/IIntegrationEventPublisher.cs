using Zaaby.DDD.Abstractions.Application;

namespace Zaaby.DDD.Abstractions.Infrastructure.EventBus
{
    public interface IIntegrationEventPublisher
    {
        void Publish<T>(T integrationEvent) where T : IIntegrationEvent;
    }
}