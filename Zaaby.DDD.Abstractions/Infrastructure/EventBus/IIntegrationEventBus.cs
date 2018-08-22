using System;
using Zaaby.DDD.Abstractions.Application;

namespace Zaaby.DDD.Abstractions.Infrastructure.EventBus
{
    public interface IIntegrationEventBus
    {
        void PublishEvent<T>(T @event) where T : IIntegrationEvent;

        void SubscribeEvent<T>(Action<T> handle) where T : IIntegrationEvent;
    }
}