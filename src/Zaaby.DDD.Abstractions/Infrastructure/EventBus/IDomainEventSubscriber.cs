using Zaaby.DDD.Abstractions.Domain;

namespace Zaaby.DDD.Abstractions.Infrastructure.EventBus
{
    public interface IDomainEventSubscriber
    {
        void Subscriber<T>(T @event) where T : IDomainEvent;
    }
}