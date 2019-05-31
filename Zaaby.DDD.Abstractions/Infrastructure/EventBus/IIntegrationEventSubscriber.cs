using System;
using Zaaby.DDD.Abstractions.Application;

namespace Zaaby.DDD.Abstractions.Infrastructure.EventBus
{
    public interface IIntegrationEventSubscriber
    {
        void Subscribe<T>(Func<Action<T>> handle) where T : IIntegrationEvent;
    }
}