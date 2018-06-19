using Zaaby.DDD.Abstractions.Infrastructure.EventBus;

namespace Zaaby.DDD.Abstractions.Application
{
    public abstract class IntegrationEventHandler<TIntegrationEvent> : IIntegrationEventHandler<TIntegrationEvent>
        where TIntegrationEvent : IIntegrationEvent
    {
        public IntegrationEventHandler(IEventBus eventBus)
        {
            eventBus.SubscribeEvent<TIntegrationEvent>(Handle);
        }

        public abstract void Handle(TIntegrationEvent integrationEvent);
    }
}