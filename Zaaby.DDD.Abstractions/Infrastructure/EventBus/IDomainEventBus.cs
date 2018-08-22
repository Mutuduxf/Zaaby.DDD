using System;
using Zaaby.DDD.Abstractions.Domain;

namespace Zaaby.DDD.Abstractions.Infrastructure.EventBus
{
    public interface IDomainEventBus
    {
        void PublishEvent<T>(T @event) where T : IDomainEvent;

        void SubscribeEvent<T>(Action<T> handle) where T : IDomainEvent;
    }
}